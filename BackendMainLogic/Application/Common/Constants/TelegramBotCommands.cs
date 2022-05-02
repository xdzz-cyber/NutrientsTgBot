using Application.TelegramBot.Queries;

namespace Application.Common.Constants;

public static class TelegramBotCommands
{
    private static Dictionary<string, Type> _commands = new Dictionary<string, Type>();

    public static Dictionary<string, Type> GetCommands()
    {
        if (!_commands.Any())
        {
            InitializeCommands();  
        }
        
        return _commands;
    }

    private static void InitializeCommands()
    {
        _commands.Add("Start", typeof(StartApplicationQuery));
    }
}
