using Application.Common.Constants;
using Telegram.Bot;


namespace TelegramBotUI;

/// <summary>
/// 
/// </summary>
public class TelegramBot
{
    private readonly IConfiguration _config;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public TelegramBot(IConfiguration configuration)
    {
        _config = configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<TelegramBotClient> GetBot()
    {
        var telegramBotClient = new TelegramBotClient(_config["Token"] ?? string.Empty);

        var hook = $"{_config["Url"]}/api/TelegramBot";
        
        await telegramBotClient.SetWebhookAsync(hook);

        await telegramBotClient.SetMyCommandsAsync(TelegramBotListOfCommandsWIthDescriptionsForSetCommands.GetCommandsList());
        
        return telegramBotClient;
    }
}
