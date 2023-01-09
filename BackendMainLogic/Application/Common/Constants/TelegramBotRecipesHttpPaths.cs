namespace Application.Common.Constants;

public static class TelegramBotRecipesHttpPaths
{
    public static readonly string GetRecipesDataByIngredients = $"https://api.spoonacular.com/recipes/findByIngredients?apiKey=1760e8784d7b4e328887ea0e6b31c7aa&ingredients=";
    
    public static readonly string GetFullRecipesData = $"https://api.spoonacular.com/recipes/id/information?includeNutrition=true&apiKey=1760e8784d7b4e328887ea0e6b31c7aa";

    public static readonly string GetRecipes = $"https://api.spoonacular.com/recipes/random?number=5&apiKey=1760e8784d7b4e328887ea0e6b31c7aa&tags=";
    
    public static readonly string GetAllRandomRecipes = $"https://api.spoonacular.com/recipes/random?number=100&apiKey=1760e8784d7b4e328887ea0e6b31c7aa&tags=";
    
    public static readonly string GetRecipeById = $"https://api.spoonacular.com/recipes/id/information?apiKey=1760e8784d7b4e328887ea0e6b31c7aa";
    
    public static readonly string GetRecipesByNutrients = "https://api.spoonacular.com/recipes/findByNutrients?number=5&apiKey=1760e8784d7b4e328887ea0e6b31c7aa" +
                                                          "&minCarbs={0}&maxCarbs={1}&minProtein={2}&maxProtein={3}&minFat={4}&maxFat={5}&minCalories={6}&maxCalories={7}";

    public static readonly string GetMealPlan = "https://api.spoonacular.com/mealplanner/generate?timeFrame=day&apiKey=1760e8784d7b4e328887ea0e6b31c7aa" +
                                                "&targetCalories={0}&diet={1}";

    public static readonly string GetRecipesWithNutrition = "https://api.spoonacular.com/recipes/informationBulk" +
                                                            "?apiKey=1760e8784d7b4e328887ea0e6b31c7aa&ids={0}&includeNutrition=true";
}
