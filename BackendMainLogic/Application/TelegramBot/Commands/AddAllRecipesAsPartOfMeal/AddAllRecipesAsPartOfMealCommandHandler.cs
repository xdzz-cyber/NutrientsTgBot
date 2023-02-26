using System.Text.Json;
using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.AddAllRecipesAsPartOfMeal;

public class AddAllRecipesAsPartOfMealCommandHandler : IRequestHandler<AddAllRecipesAsPartOfMealCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddAllRecipesAsPartOfMealCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager, 
        HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _ctx = ctx;
        _userManager = userManager;
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<string> Handle(AddAllRecipesAsPartOfMealCommand request, CancellationToken cancellationToken)
    {
        var mealsIds = JsonSerializer.Deserialize<List<Recipe>>(_httpContextAccessor.HttpContext!.Session
            .GetString("CurrentRecipes")!)!.Select(r => r.Id).ToList();

        var recipesOfUserToBeAddedCounter = 0;
        
        if (!mealsIds.Any())
        {
            return "Please, get meal plan before saving meals.";
        }

        var userInfo = await _userManager.FindByNameAsync(request.Username);

        foreach (var mealId in mealsIds)
        {
            if (await _ctx.RecipesUsers
                    .FirstOrDefaultAsync(ru => ru.RecipeId == mealId && ru.AppUserId 
                        == userInfo.Id, cancellationToken) is null 
                && await _ctx.Recipes.FirstOrDefaultAsync(r => r.Id == mealId,
                    cancellationToken: cancellationToken) is null)
            {
                var recipeToBeAddedHttpMessage = await _httpClient
                    .GetAsync(TelegramBotRecipesHttpPaths.GetRecipeById.Replace("id", mealId.ToString()),
                        cancellationToken);

                var recipeToBeAdded = JsonSerializer
                    .Deserialize<Recipe>(await recipeToBeAddedHttpMessage.Content.ReadAsStringAsync(cancellationToken));

                await _ctx.Recipes.AddAsync(recipeToBeAdded!, cancellationToken);

            } else if (await _ctx.RecipesUsers
                           .FirstOrDefaultAsync(ru => ru.RecipeId == mealId && ru.AppUserId
                               == userInfo.Id, cancellationToken) is null)
            {
                var mealInfoFromRecipeWithSameId = await _ctx.Recipes
                    .FirstOrDefaultAsync(r => r.Id == mealId, cancellationToken: cancellationToken);

                if (_ctx.RecipesUsers.Count() + recipesOfUserToBeAddedCounter < 
                    TelegramBotRecipesPerUserAmount.MaxRecipesPerUser)
                {
                    await _ctx.RecipesUsers.AddAsync(new RecipesUsers
                    {
                        AppUserId = userInfo.Id,
                        RecipeId = mealInfoFromRecipeWithSameId!.Id,
                        IsPartOfTheMeal = true
                    }, cancellationToken);

                    recipesOfUserToBeAddedCounter += 1;
                }
            }
            else
            {
                var recipe = _ctx.RecipesUsers.First(ru => ru.RecipeId == mealId && ru.AppUserId == userInfo.Id);

                recipe.IsPartOfTheMeal = true;
            }
        }

        await _ctx.SaveChangesAsync(cancellationToken);

        return "Everything has been successfully saved as part of the meal.";
    }
}
