using System.Globalization;
using Application.Common.Constants;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotUI.Controllers;

[Route("api/[controller]")]
public class TelegramBotController : BaseController
{
    private readonly TelegramBotClient _telegramBotClient;
    private readonly Dictionary<string, Type> _telegramBotCommands;
    private static readonly List<Type> _lastExecutedCommandsTypes = new ();

    public TelegramBotController(TelegramBot telegramBot)
    {
        _telegramBotClient = telegramBot.GetBot().Result;
        _telegramBotCommands = TelegramBotCommands.GetCommands();
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] object update)
    {
        var upd = JsonConvert.DeserializeObject<Update>(update.ToString());

        var chat = upd.Message?.Chat;

        if (chat == null)
        {
            return BadRequest("No chat found");
        }
        
        try
        {
            var command = $"{upd.Message?.Text!.Replace("/", "").FirstCharToUpper()}";

            if (command.All(char.IsDigit) && !_lastExecutedCommandsTypes.Any())
            {
                await _telegramBotClient.SendTextMessageAsync(chat.Id, "Bad request.",
                    ParseMode.Markdown, replyMarkup: GetButton());
                return Ok();
            }

            var queryType = command.All(char.IsDigit)
                ? _lastExecutedCommandsTypes.Last(x => x.GetInterface(nameof(ICommand)) != null)
                : _telegramBotCommands[command];

            var ctorParamsTypes = new List<dynamic>();

            var allParams = new List<dynamic> {chat.Username!, chat.Id, command.All(char.IsDigit) 
                ? Convert.ToDouble(command, CultureInfo.InvariantCulture) : command};
            
            var ctors = queryType.GetConstructors();
            // assuming class A has only one constructor
            var ctor = ctors[0];
            
            var ctorParams = new List<dynamic?>(); 
            
            foreach (var param in ctor.GetParameters())
            {
                ctorParamsTypes.Insert(param.Position, param.ParameterType);
            }

            foreach (var ctorParamType in ctorParamsTypes)
            {

                ctorParams.Add(allParams.FirstOrDefault(x => x.GetType().Equals(ctorParamType))
                               ?? ctor.GetParameters().FirstOrDefault(x
                                   => x.ParameterType.Equals(ctorParamType))!.DefaultValue);
            }

            var instance = ctor.Invoke(ctorParams.ToArray());

            var queryResult =
                JsonConvert.SerializeObject(await Mediator?.Send(instance ?? throw new InvalidOperationException())!);

            await _telegramBotClient.SendTextMessageAsync(chat.Id, queryResult,
                ParseMode.Markdown, replyMarkup: GetButton());

            if (!command.All(char.IsDigit))
            {
                _lastExecutedCommandsTypes.AddRange(new []{_telegramBotCommands[command],
                    _telegramBotCommands.FirstOrDefault(x => 
                        AreTwoStringsHaveCommonSubstring(x.Key, command) && x.Value.GetInterface(nameof(ICommand)) != null).Value});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await _telegramBotClient.SendTextMessageAsync(chat.Id, e.Message,
                ParseMode.Markdown, replyMarkup: GetButton());
        }

        return Ok();
    }

    private IReplyMarkup GetButton()
    {
        var keyboardCommands = new List<List<KeyboardButton>>();

        for (var i = 0; i < _telegramBotCommands.Keys.Count; i+=2)
        {
            keyboardCommands.Add(_telegramBotCommands.Keys.Count == i + 1 ?
                new List<KeyboardButton>{new (_telegramBotCommands.Keys.ToArray()[i])}:
                new List<KeyboardButton>{new (_telegramBotCommands.Keys.ToArray()[i]),
                new (_telegramBotCommands.Keys.ToArray()[i+1])});
        }
        
        return new ReplyKeyboardMarkup(keyboardCommands){ResizeKeyboard = true};
    }

    private bool AreTwoStringsHaveCommonSubstring(string s1, string s2)
    {
        var sameLengthCounter = 0;
        var i = 1;
        while (!(s1.Length - i < 0 || s2.Length- i < 0) && s1[^i].Equals(s2[^i]))
        {
            sameLengthCounter += 1;
            i += 1;
        }

        return sameLengthCounter >= (int) StringMaxSubstringLengthToBeEqualWithAnother.MaxLength;
    }
}
