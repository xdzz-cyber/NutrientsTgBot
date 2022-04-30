using Telegram.Bot;

namespace TelegramBotUI;

public class TelegramBot
{
    private readonly IConfiguration _config;

    public TelegramBot(IConfiguration configuration)
    {
        _config = configuration;
    }

    public async Task<TelegramBotClient> GetBot()
    {
        var telegramBotClient = new TelegramBotClient(_config["Token"] ?? string.Empty);

        var hook = $"{_config["Url"]}/api/TelegramBot";
        
        await telegramBotClient.SetWebhookAsync(hook);

        return telegramBotClient;
    }
}
