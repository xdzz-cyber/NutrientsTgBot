using Telegram.Bot.Types;

namespace Application.Common.Constants;

public static class TelegramBotListOfCommandsWIthDescriptionsForSetCommands
{
    public static List<BotCommand> GetCommandsList()
    {
        return new List<BotCommand>
        {
            new() {Command = "start", Description = "start application and user is being authorized"},
            new() {Command = "get_approved_amount_of_nutrients", Description = "gets approved amount of nutrients"},
            new() {Command = "get_my_weight", Description = "gets weight of user"},
            new() {Command = "get_user_info", Description = "gets sex, age and height of user"},
            new() {Command = "update_my_weight", Description = "updates weight"},
            new() {Command = "update_user_age", Description = "updates age"},
            new() {Command = "update_user_tallness", Description = "updates tallness"},
            new() {Command = "update_user_gender", Description = "updates gender"},
            new() {Command = "get_user_water_balance_level", Description = "gets water balance level"},
            new() {Command = "update_user_water_balance_level", Description = "updates water balance level"},
            new() {Command = "get_recipes_by_ingredients", Description = "gets recipes by ingredients"},
            new() {Command = "get_user_recipe_list", Description = "gets liked recipes"},
            new() {Command = "get_recipes_as_part_of_meal", Description = "gets liked recipes as part of the meal"},
            new() {Command = "get_user_filters_for_recipes", Description = "gets user filters for recipes"},
            new() {Command = "get_recipes_by_nutrients", Description = "gets recipes by nutrients"},
            new() {Command = "update_user_nutrients_plan", Description = "updates basic nutrients"},
            new() {Command = "get_user_nutrients_plan", Description = "gets statistics for nutrients plan"},
            new() {Command = "get_meal_plan_for_user", Description = "gets meal plan"},
            new() {Command = "get_user_supplements_outline", Description = "gets nutrients plan report"}
        };
    }
}
