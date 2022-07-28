namespace Domain.TelegramBotEntities;

public static class StateManagement
{
    public static Dictionary<string, string> TempData;
    static StateManagement() => TempData = new Dictionary<string, string>();
}
