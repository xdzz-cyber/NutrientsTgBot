using System.Text.Json;
using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;

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
        var rawRandomRecipes = await _httpClient.GetAsync(TelegramBotRecipesHttpPaths.GetAllRandomRecipes, cancellationToken);
        var randomRecipes = new RecipesList();
        
        if (rawRandomRecipes.IsSuccessStatusCode)
        {
            randomRecipes = JsonSerializer.Deserialize<RecipesList>(await rawRandomRecipes.Content.ReadAsStringAsync(cancellationToken));
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
                    Vegetarian = recipe.Vegetarian
                };
               await _ctx.Recipes.AddAsync(newRecipe, cancellationToken);
            }
        }

        await _ctx.SaveChangesAsync(cancellationToken);
    }
}
