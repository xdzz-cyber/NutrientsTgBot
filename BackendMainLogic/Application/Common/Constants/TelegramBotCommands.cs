using Application.TelegramBot.Commands.AddRecipeAsPartOfMeal;
using Application.TelegramBot.Commands.AddRecipeToUser;
using Application.TelegramBot.Commands.RemoveRecipeFromLikedList;
using Application.TelegramBot.Commands.RemoveRecipeFromTheMeal;
using Application.TelegramBot.Commands.UpdateUserWaterBalanceLevel;
using Application.TelegramBot.Commands.UpdateUserWeight;
using Application.TelegramBot.Queries;
using Application.TelegramBot.Queries.GetRecipesByIngredients;
using Application.TelegramBot.Queries.GetUserRecipeList;
using Application.TelegramBot.Queries.GetUserWaterBalanceLevel;
using Application.TelegramBot.Queries.GetUserWeight;
using Domain.TelegramBotEntities;

namespace Application.Common.Constants;

public static class TelegramBotCommands
{
    private static readonly Dictionary<string, TelegramBotCommand> _commands = new ();

    public static Dictionary<string, TelegramBotCommand> GetCommands()
    {
        if (!_commands.Any())
        {
            InitializeCommands();  
        }
        
        return _commands;
    }

    private static void InitializeCommands()
    {
        _commands.Add("Start", new TelegramBotCommand(typeof(StartApplicationQuery), true));
        _commands.Add("GetMyWeight", new TelegramBotCommand(typeof(GetUserWeightQuery), true));
        _commands.Add("UpdateMyWeight", new TelegramBotCommand(typeof(UpdateAppUserWeightCommand), true));
        _commands.Add("GetUserWaterBalanceLevel", new TelegramBotCommand(typeof(GetUserWaterBalanceLevelQuery), true));
        _commands.Add("UpdateUserWaterBalanceLevel", new TelegramBotCommand(typeof(UpdateUserWaterBalanceLevelCommand), true));
        _commands.Add("GetRecipesByIngredients", new TelegramBotCommand(typeof(GetRecipesByIngredientsQuery), true));
        _commands.Add("AddRecipeToUser", new TelegramBotCommand(typeof(AddRecipeToUserCommand), false));
        _commands.Add("GetUserRecipeList", new TelegramBotCommand(typeof(GetUserRecipeListQuery), true));
        _commands.Add("AddRecipeAsPartOfMeal", new TelegramBotCommand(typeof(AddRecipeAsPartOfMealCommand), false));
        _commands.Add("RemoveRecipeFromTheMeal", new TelegramBotCommand(typeof(RemoveRecipeFromTheMealCommand), false));
        _commands.Add("RemoveRecipeFromLikedList", new TelegramBotCommand(typeof(RemoveRecipeFromLikedListCommand), false));
        // _commands.Add("ClearLikedRecipesList", typeof(ClearLikedRecipesList));
        // _commands.Add("ClearMealOffRecipes", typeof(ClearMealOffRecipes));
        // _commands.Add("AddAllRecipesAsMeal", typeof(AddAllRecipesAsMeal));
        // Filters and find by nutrients will be here
    }
    
}
