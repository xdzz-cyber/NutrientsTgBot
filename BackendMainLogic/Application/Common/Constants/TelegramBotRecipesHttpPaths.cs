namespace Application.Common.Constants;

public static class TelegramBotRecipesHttpPaths
{
    public static readonly string GetRecipesDataByIngredients = $"https://api.spoonacular.com/recipes/findByIngredients?";
    
    public static readonly string GetFullRecipesData = $"https://api.spoonacular.com/recipes/id/information?includeNutrition=true&";

    public static readonly string GetRecipes = $"https://api.spoonacular.com/recipes/random?number=5&&tags=";
    
    public static readonly string GetAllRandomRecipes = $"https://api.spoonacular.com/recipes/random?number=100&";
    
    public static readonly string GetRecipeById = $"https://api.spoonacular.com/recipes/id/information?";
    
    public static readonly string GetRecipesByNutrients = "https://api.spoonacular.com/recipes/findByNutrients?number=5&" +
                                                          "&minCarbs={0}&maxCarbs={1}&minProtein={2}&maxProtein={3}&minFat={4}&maxFat={5}&minCalories={6}&maxCalories={7}";

    public static readonly string GetMealPlan = "https://api.spoonacular.com/mealplanner/generate?timeFrame=day&" +
                                                "&targetCalories={0}&diet={1}";

    public static readonly string GetRecipesWithNutrition = "https://api.spoonacular.com/recipes/informationBulk" +
                                                            "?&ids={0}&includeNutrition=true";
}
