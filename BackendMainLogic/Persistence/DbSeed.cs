using System.Text.Json;
using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using Domain.TelegramBotEntities.RecipesNutrition;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DbSeed
{
    private readonly HttpClient _httpClient;
    private readonly ITelegramBotDbContext _ctx;

    public DbSeed(HttpClient httpClient, ITelegramBotDbContext ctx)
    {
        _httpClient = httpClient;
        _ctx = ctx;
    }

    public async Task Seed(CancellationToken cancellationToken)
    {
        await SeedRandomRecipes(cancellationToken);
        await SeedRandomRecipesNutrients(cancellationToken);
    }

    private async Task SeedRandomRecipes(CancellationToken cancellationToken)
    {
        for (var i = 0; i < 15; i++)
        {
            var rawRandomRecipes = await _httpClient
                .GetAsync(TelegramBotRecipesHttpPaths.GetAllRandomRecipes, cancellationToken);

            var randomRecipes = new RecipesList();

            if (rawRandomRecipes.IsSuccessStatusCode)
            {
                randomRecipes = JsonSerializer
                    .Deserialize<RecipesList>(await rawRandomRecipes.Content.ReadAsStringAsync(cancellationToken));
            }

            foreach (var recipe in randomRecipes!.Recipes)
            {
                if (!_ctx.Recipes.Any(r => r.Id == recipe.Id))
                {
                    var newRecipe = new Recipe
                    {
                        Id = recipe.Id,
                        AggregateLikes = recipe.AggregateLikes,
                        CookingMinutes = recipe.CookingMinutes,
                        GlutenFree = recipe.GlutenFree,
                        HealthScore = recipe.HealthScore,
                        PricePerServing = recipe.PricePerServing,
                        SourceName = recipe.SourceName ?? string.Empty,
                        SpoonacularSourceUrl = recipe.SpoonacularSourceUrl,
                        Title = recipe.Title,
                        Vegetarian = recipe.Vegetarian,
                        SourceUrl = recipe.SourceUrl
                    };
                    await _ctx.Recipes.AddAsync(newRecipe, cancellationToken);
                }
            }
            
            await _ctx.SaveChangesAsync(cancellationToken);
            
            Thread.Sleep(1000);
        }
        
    }

    private async Task SeedRandomRecipesNutrients(CancellationToken cancellationToken)
    {
        for (var i = 0; i < 15; i++)
        {
            var recipesToBeUpdatedWithNutrients = await _ctx.Recipes
                .Where(recipe => _ctx.Meals.Any(meal => meal.RecipeId == recipe.Id) == false)
                .Select(r => r.Id).Take(100).ToListAsync(cancellationToken: cancellationToken);

            var _ = string.Format(TelegramBotRecipesHttpPaths.GetRecipesWithNutrition,
                string.Join(",", recipesToBeUpdatedWithNutrients));

            var recipesNutritionHttpMessage = await _httpClient
                .GetAsync(_, cancellationToken);

            var recipesNutritionData = await recipesNutritionHttpMessage.Content.ReadAsStringAsync(cancellationToken);

            var recipes = JsonSerializer.Deserialize<List<RecipesNutrition>>(recipesNutritionData);

            var nutrients = await _ctx.Nutrients.ToListAsync(cancellationToken);

            foreach (var recipe in recipes!)
            {
                var neededNutrients = recipe.Nutrition.Nutrients
                    .Where(rn => nutrients.FirstOrDefault(n =>
                        n.Name.Equals(rn.Name)) != null).ToList();

                await _ctx.Meals.AddAsync(new Meal
                {
                    Calories = neededNutrients
                        .FirstOrDefault(nu => nu.Name.Equals("Calories"))!.Amount.ToString(),
                    Carbs = neededNutrients
                        .FirstOrDefault(nu => nu.Name.Equals("Carbohydrates"))!.Amount.ToString(),
                    Fat = neededNutrients
                        .FirstOrDefault(nu => nu.Name.Equals("Fat"))!.Amount.ToString(),
                    Protein = neededNutrients
                        .FirstOrDefault(nu => nu.Name.Equals("Protein"))!.Amount.ToString(),
                    RecipeId = recipe.Id
                }, cancellationToken);
            }
            
            await _ctx.SaveChangesAsync(cancellationToken);
            
            Thread.Sleep(1000);
        }
        
    }
}
