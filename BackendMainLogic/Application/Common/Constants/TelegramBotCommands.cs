using Application.TelegramBot.Commands.UpdateUserWaterBalanceLevel;
using Application.TelegramBot.Commands.UpdateUserWeight;
using Application.TelegramBot.Queries;
using Application.TelegramBot.Queries.GetRecipesByIngredients;
using Application.TelegramBot.Queries.GetUserWaterBalanceLevel;
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
        _commands.Add("GetUserWaterBalanceLevel", typeof(GetUserWaterBalanceLevelQuery));
        _commands.Add("UpdateUserWaterBalanceLevel", typeof(UpdateUserWaterBalanceLevelCommand));
        _commands.Add("GetRecipesByIngredients", typeof(GetRecipesByIngredientsQuery));
    }
}
