using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.TelegramBotEntities;
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
    private readonly Dictionary<string, TelegramBotCommand> _telegramBotCommands;
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
            
             //BadRequest("No chat found");
             throw new NotFoundException("Telegram bot chat was not found.", upd.Id);
             //return Ok("No chat found");
        }

        try
        {
            var command = $"{upd.Message?.Text!.Replace("/", "").Replace("_", "").Trim().FirstCharToUpper()}";

            if (!_telegramBotCommands.Any(c => AreTwoStringsHaveCommonSubstring(c.Key, command)) && !_lastExecutedCommandsTypes.Any()) //command.All(char.IsDigit)
            {
                await _telegramBotClient.SendTextMessageAsync(chat.Id, "Bad request.",
                    ParseMode.Markdown, replyMarkup: GetButton());
                
                throw new NotFoundException("No command that satisfies conditions was found.", upd.Id); //return Ok("No commands found");
            }

            var queryType = _telegramBotCommands.Any(c => AreTwoStringsHaveCommonSubstring(c.Key, command))
                ? _telegramBotCommands.First(c => AreTwoStringsHaveCommonSubstring(c.Key, command)).Value.Type //_telegramBotCommands[Regex.Replace(command, @"[0-9_]+", string.Empty)]
                : _lastExecutedCommandsTypes.Last(x =>
                    x != null && x.GetInterfaces().Any(i => i.Name == nameof(ICommand) || i.Name == nameof(IQuery))); //GetInterface(nameof(ICommand)) != null || x.GetInterface(nameof(IQuery)) != null
                

            // var queryType = _telegramBotCommands.FirstOrDefault(c => AreTwoStringsHaveCommonSubstring(c.Key, command));
            //
            // if (!string.IsNullOrEmpty(queryType.Key))
            // {
            //     queryType = _telegramBotCommands[queryType.Key];
            // }
            //      _lastExecutedCommandsTypes.Last(x => x != null && x.GetInterfaces().Any(i => i.Name == nameof(ICommand) || i.Name == nameof(IQuery)));

            var ctorParamsTypes = new List<object>();

            var allParams = new List<object> {chat.Username!, chat.Id, command.All(char.IsDigit) ? double.Parse(command) : command};
            
            var ctors = queryType.GetConstructors();
            // assuming class A has only one constructor
            var ctor = ctors[0];
            
            var ctorParams = new List<object?>();

            foreach (var param in ctor.GetParameters())
            {
                ctorParamsTypes.Insert(param.Position, param.ParameterType);
            }

            foreach (var ctorParamType in ctorParamsTypes)
            {
                ctorParams.Add(allParams.FirstOrDefault(x => x.GetType().Equals(ctorParamType) && !ctorParams.Any(ctp => ctp!.GetHashCode().Equals(x.GetHashCode()))) ?? GetDefault((Type) ctorParamType));
                //     &&
                // !ctorParams.Any(element => element != null && element.GetHashCode() == x.GetHashCode())) ?? GetDefault((Type) ctorParamType)
                //var f = GetDefaultValue((ctorParamType as Type)!);
                //var s = allParams.FirstOrDefault(x => x.GetType().Equals(ctorParamType)) ?? GetDefault((Type) ctorParamType);
                // var el = allParams.FirstOrDefault(x =>
                //     GetDefaultValue((x as Type)) == GetDefaultValue((ctorParamType as Type)) &&
                //     !ctorParams.Any(element => element != null && element.GetHashCode() == x.GetHashCode()));
                // ctorParams.Add(el); 
                
                //x.GetType().Equals(ctorParamType))
                // ?? GetDefault(ctorParamType)
            }

            var instance = ctor.Invoke(ctorParams.ToArray());

            // var queryResult =
            //     JsonConvert.SerializeObject();

            var queryResult = await Mediator?.Send(instance ?? throw new InvalidOperationException())!;
            
            await _telegramBotClient.SendTextMessageAsync(chat.Id, queryResult!.ToString()!,
                parseMode: ParseMode.Html, replyMarkup: GetButton());

            if (_telegramBotCommands.Any(c => AreTwoStringsHaveCommonSubstring(c.Key, command)))
            {
                // _lastExecutedCommandsTypes.AddRange(new []{_telegramBotCommands[command].Type,
                //     _telegramBotCommands.FirstOrDefault(x => 
                //         AreTwoStringsHaveCommonSubstring(x.Key, command) && 
                //         x.Value.Type.GetInterface(nameof(ICommand)) != null)
                //         .Value.Type});
                _lastExecutedCommandsTypes.Add(_telegramBotCommands.FirstOrDefault(c => AreTwoStringsHaveCommonSubstring(c.Key, command)).Value.Type);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await _telegramBotClient.SendTextMessageAsync(chat.Id, e.Message,
                ParseMode.Html, replyMarkup: GetButton());
        }

        return Ok();
    }

    private IReplyMarkup GetButton()
    {
        var keyboardCommands = new List<List<KeyboardButton>>();
        var keyboardsCommandsCount = _telegramBotCommands
            .Count(command => command.Value.IsVisibleAsPartOfUserInterface);
        var visibleTelegramBotCommands = _telegramBotCommands.Keys.Where(key => _telegramBotCommands[key].IsVisibleAsPartOfUserInterface).ToList();
        
        for (var i = 0; i < keyboardsCommandsCount; i+=2) //_telegramBotCommands.Keys.Count
        {
            keyboardCommands.Add(i + 1 != keyboardsCommandsCount ?  new List<KeyboardButton>
            {
                new(visibleTelegramBotCommands[i]),
                new(visibleTelegramBotCommands[i + 1])
            } : new List<KeyboardButton>{visibleTelegramBotCommands[i]});
           
            // if (_telegramBotCommands[_telegramBotCommands.Keys.ToArray()[i]].IsVisibleAsPartOfUserInterface)
            // {
            //     keyboardCommands.Add(_telegramBotCommands.Keys.Count == i + 1
            //         ? new List<KeyboardButton> {new(_telegramBotCommands.Keys.ToArray()[i])}
            //         : (_telegramBotCommands[_telegramBotCommands.Keys.ToArray()[i + 1]].IsVisibleAsPartOfUserInterface
            //             ? new List<KeyboardButton>
            //             {
            //                 new(_telegramBotCommands.Keys.ToArray()[i]),
            //                 new(_telegramBotCommands.Keys.ToArray()[i + 1])
            //             }
            //             : new List<KeyboardButton>{new(_telegramBotCommands.Keys.ToArray()[i])}));
            // }
            // else
            // {
            //     i -= 1;
            // }
        }
        
        return new ReplyKeyboardMarkup(keyboardCommands){ResizeKeyboard = true};
    }

    private bool AreTwoStringsHaveCommonSubstring(string s1, string s2)
    {
        var sameLengthCounter = 0;
        var i = 1;
        while (!(s1.Length - i < 0 || s2.Length- i < 0))
        {
            if(s1[i-1] == s2[i-1]){
                sameLengthCounter += 1;
            }
            
            i += 1;
        }

        var minimalLengthToBeEqual = s1.Length > s2.Length ? s1.Length * 0.65 : s2.Length * 0.65;
        return sameLengthCounter >= minimalLengthToBeEqual;
    }
    
    public T GetDefaultGeneric<T>()
    {
        return default(T);
    }
    
    public object GetDefault(Type t)
    {
        return this.GetType().GetMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(this, null);
    }
    
    public object GetDefaultValue(Type t)
    {
        if (t.IsValueType)
            return Activator.CreateInstance(t);

        return null;
    }
}

