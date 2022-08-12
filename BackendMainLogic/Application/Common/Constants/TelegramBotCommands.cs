using Application.TelegramBot.Commands.AddAllLikedRecipesAsMeal;
using Application.TelegramBot.Commands.AddAllRecipesAsPartOfMeal;
using Application.TelegramBot.Commands.AddRecipeAsPartOfMeal;
using Application.TelegramBot.Commands.AddRecipeFiltersToUser;
using Application.TelegramBot.Commands.AddRecipeToUser;
using Application.TelegramBot.Commands.ClearLikedRecipesList;
using Application.TelegramBot.Commands.ClearRecipesAsPartOfMeal;
using Application.TelegramBot.Commands.RemoveRecipeFromLikedList;
using Application.TelegramBot.Commands.RemoveRecipeFromTheMeal;
using Application.TelegramBot.Commands.TurnOffAllRecipeFiltersOfUser;
using Application.TelegramBot.Commands.TurnOffRecipeFilterOfUser;
using Application.TelegramBot.Commands.TurnOnAllRecipeFiltersOfUser;
using Application.TelegramBot.Commands.UpdateUserNutrientsPlan;
using Application.TelegramBot.Commands.UpdateUserWaterBalanceLevel;
using Application.TelegramBot.Commands.UpdateUserWeight;
using Application.TelegramBot.Queries;
using Application.TelegramBot.Queries.GetMealPlanForUser;
using Application.TelegramBot.Queries.GetRecipesAsPartOfMeal;
using Application.TelegramBot.Queries.GetRecipesByIngredients;
using Application.TelegramBot.Queries.GetRecipesByNutrients;
using Application.TelegramBot.Queries.GetUserFiltersForRecipes;
using Application.TelegramBot.Queries.GetUserNutrientsCompletePlanReport;
using Application.TelegramBot.Queries.GetUserNutrientsPlan;
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
        _commands.Add("ClearLikedRecipesList", new TelegramBotCommand(typeof(ClearLikedRecipesListCommand), false));
        _commands.Add("ClearRecipesAsPartOfMeal", new TelegramBotCommand(typeof(ClearRecipesAsPartOfMealCommand), false));
        _commands.Add("GetRecipesAsPartOfMeal", new TelegramBotCommand(typeof(GetRecipesAsPartOfMealQuery), true));
        _commands.Add("AddAllLikedRecipesAsMeal", new TelegramBotCommand(typeof(AddAllLikedRecipesAsMealCommand), false));
        _commands.Add("GetUserFiltersForRecipes", new TelegramBotCommand(typeof(GetUserFiltersForRecipesQuery), true));
        _commands.Add("AddRecipeFiltersToUser",new TelegramBotCommand(typeof(AddRecipeFiltersToUserCommand), false));
        _commands.Add("TurnOffRecipeFilterOfUser", new TelegramBotCommand(typeof(TurnOffRecipeFilterOfUserCommand),false));
        _commands.Add("TurnOffAllRecipeFiltersOfUser", new TelegramBotCommand(typeof(TurnOffAllRecipeFiltersOfUserCommand),false));
        _commands.Add("TurnOnAllRecipeFiltersOfUser", new TelegramBotCommand(typeof(TurnOnAllRecipeFiltersOfUserCommand), false));
        _commands.Add("GetRecipesByNutrients", new TelegramBotCommand(typeof(GetRecipesByNutrientsQuery), true));
        _commands.Add("UpdateUserNutrientsPlan", new TelegramBotCommand(typeof(UpdateUserNutrientsPlanCommand), true));
        _commands.Add("GetUserNutrientsPlan", new TelegramBotCommand(typeof(GetUserNutrientsPlanQuery), true));
        _commands.Add("GetMealPlanForUser", new TelegramBotCommand(typeof(GetMealPlanForUserQuery), true));
        _commands.Add("AddAllRecipesAsPartOfMeal", new TelegramBotCommand(typeof(AddAllRecipesAsPartOfMealCommand), false));
        _commands.Add("GetUserNutrientsCompletePlanReport", new TelegramBotCommand(typeof(GetUserNutrientsCompletePlanReportQuery), true));
        // Filters, find by nutrients, get meal and get fat,carbohydrates,protein for period of time will be here
        
        // 26 queries and commands
    }
    
}
