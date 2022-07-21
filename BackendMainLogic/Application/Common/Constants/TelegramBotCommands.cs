using Application.TelegramBot.Commands.AddRecipeAsPartOfMeal;
using Application.TelegramBot.Commands.AddRecipeToUser;
using Application.TelegramBot.Commands.RemoveRecipeFromTheMeal;
using Application.TelegramBot.Commands.UpdateUserWaterBalanceLevel;
using Application.TelegramBot.Commands.UpdateUserWeight;
using Application.TelegramBot.Queries;
using Application.TelegramBot.Queries.GetRecipesByIngredients;
using Application.TelegramBot.Queries.GetUserRecipeList;
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
        _commands.Add("AddRecipeToUser", typeof(AddRecipeToUserCommand));
        _commands.Add("GetUserRecipeList", typeof(GetUserRecipeListQuery));
        _commands.Add("AddRecipeAsPartOfMeal", typeof(AddRecipeAsPartOfMealCommand));
        _commands.Add("RemoveRecipeFromTheMeal", typeof(RemoveRecipeFromTheMealCommand));
        // _commands.Add("RemoveFromLikedList", typeof(RemoveFromLikedList));
        // _commands.Add("ClearLikedRecipesList", typeof(ClearLikedRecipesList));
        // _commands.Add("ClearMealOffRecipes", typeof(ClearMealOffRecipes));
        // _commands.Add("AddAllRecipesAsMeal", typeof(AddAllRecipesAsMeal));
        // Filters and find by nutrients will be here
    }
    
}
