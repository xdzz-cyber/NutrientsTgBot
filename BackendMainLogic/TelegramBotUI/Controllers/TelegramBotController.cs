using System.Reflection;
using Application.Common.Constants;
using Application.Users.Commands.CreateUser;
using Application.Users.Queries.GetUserList;
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
    private static readonly List<Type> _lastExecutedCommandsTypes = new List<Type>();

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
        
        
        var command =  $"{upd.Message?.Text!.Replace("/", "").FirstCharToUpper()}";
        
        var queryType = command.All(char.IsDigit) ? _lastExecutedCommandsTypes.Last() : _telegramBotCommands[command];
        
        var ctor = queryType.GetConstructor(new[] {typeof(string), typeof(long)});
        
        var instance = ctor?.Invoke(new object[] {chat.Username!, chat.Id });
        
        var queryResult = JsonConvert.SerializeObject(await Mediator?.Send(instance ?? throw new InvalidOperationException())!);

        await _telegramBotClient.SendTextMessageAsync(chat.Id, queryResult,
            ParseMode.Markdown, replyMarkup: GetButton());
        
        _lastExecutedCommandsTypes.Add(_telegramBotCommands[command]);

        return Ok();
    }

    private IReplyMarkup GetButton()
    {
        var keyboardCommands = new List<List<KeyboardButton>>();
        
        foreach (var key in _telegramBotCommands.Keys)
        {
            keyboardCommands.Add(new List<KeyboardButton>() {new (key)});
        }
        
        return new ReplyKeyboardMarkup(keyboardCommands){ResizeKeyboard = true};
    }
}
