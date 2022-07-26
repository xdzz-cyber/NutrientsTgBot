namespace Application.Common.Constants;

public static class TelegramBotRecipeFilters
{
    public static readonly List<string> RecipeFilters;

    static TelegramBotRecipeFilters()
    {
        RecipeFilters = new List<string>()
        {
            "GlutenFree",
            "Vegetarian"
        };
    }
}
