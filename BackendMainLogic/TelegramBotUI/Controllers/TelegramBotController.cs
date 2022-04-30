using System.Reflection;
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


    public TelegramBotController(TelegramBot telegramBot)
    {
        _telegramBotClient = telegramBot.GetBot().Result;
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

        var command = $"{upd.Message?.Text}Query";

        var typeOfQuery = Assembly.GetAssembly(typeof(GetUserListQuery))?.GetTypes()
            .FirstOrDefault(x => x.Name.Contains(command));

        var query = Activator.CreateInstance(typeOfQuery!);

        var queryResult = JsonConvert.SerializeObject(await Mediator?.Send(query!)!);

        await _telegramBotClient.SendTextMessageAsync(chat.Id, queryResult,
            ParseMode.Markdown, replyMarkup: GetButton());

        return Ok();
    }

    private static IReplyMarkup GetButton()
    {
        var keyboard = new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
        {
            new List<KeyboardButton>() {new KeyboardButton("GetUserList")}
        }) {ResizeKeyboard = true};
        return keyboard;
    }
}
