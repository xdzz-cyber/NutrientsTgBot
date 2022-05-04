using Application.TelegramBot.Commands.UpdateAppUserWeight;
using Application.TelegramBot.Queries;
using Application.TelegramBot.Queries.GetUserWeight;

namespace Application.Common.Constants;

public static class TelegramBotCommands
{
    private static Dictionary<string, Type> _commands = new ();

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
        _commands.Add("GetMyWeight", typeof(GetUserWeightQuery));
        _commands.Add("UpdateMyWeight", typeof(UpdateAppUserWeightCommand));
    }
}
