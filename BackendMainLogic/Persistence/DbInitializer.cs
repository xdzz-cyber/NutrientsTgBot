namespace Persistence;

public static class DbInitializer
{
    public static void Initialize(TelegramBotDbContext ctx)
    {
        ctx.Database.EnsureCreated();
    }
}
