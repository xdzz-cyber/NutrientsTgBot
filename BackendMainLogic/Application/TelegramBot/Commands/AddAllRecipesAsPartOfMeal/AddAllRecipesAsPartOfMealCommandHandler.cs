using System.Text.Json;
using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.AddAllRecipesAsPartOfMeal;

public class AddAllRecipesAsPartOfMealCommandHandler : IRequestHandler<AddAllRecipesAsPartOfMealCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;
    private readonly HttpClient _httpClient;

    public AddAllRecipesAsPartOfMealCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager, 
        HttpClient httpClient)
    {
        _ctx = ctx;
        _userManager = userManager;
        _httpClient = httpClient;
    }
    
    public async Task<string> Handle(AddAllRecipesAsPartOfMealCommand request, CancellationToken cancellationToken)
    {
        var mealsIds = StateManagement.TempData["MealsIds"].Split(',');

        var recipesOfUserToBeAddedCounter = 0;
        
        if (!mealsIds.Any())
        {
            return "Please, get meal plan before saving meals.";
        }

        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }
        
        foreach (var mealId in mealsIds)
        {
            if (await _ctx.RecipesUsers
                    .FirstOrDefaultAsync(ru => ru.RecipeId.ToString() == mealId && ru.AppUserId 
                        == userInfo.Id, cancellationToken) is null 
                && await _ctx.Recipes.FirstOrDefaultAsync(r => r.Id.ToString() == mealId,
                    cancellationToken: cancellationToken) is null)
            {
                var recipeToBeAddedHttpMessage = await _httpClient
                    .GetAsync(TelegramBotRecipesHttpPaths.GetRecipeById.Replace("id", mealId),
                        cancellationToken);

                var recipeToBeAdded = JsonSerializer
                    .Deserialize<Recipe>(await recipeToBeAddedHttpMessage.Content.ReadAsStringAsync(cancellationToken));

                await _ctx.Recipes.AddAsync(recipeToBeAdded!, cancellationToken);

                await _ctx.SaveChangesAsync(cancellationToken);
                
            } else if (await _ctx.RecipesUsers
                           .FirstOrDefaultAsync(ru => ru.RecipeId.ToString() == mealId && ru.AppUserId
                               == userInfo.Id, cancellationToken) is null)
            {
                var mealInfoFromRecipeWithSameId = await _ctx.Recipes
                    .FirstOrDefaultAsync(r => r.Id.ToString() == mealId, cancellationToken: cancellationToken);

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
        }

        await _ctx.SaveChangesAsync(cancellationToken);

        return "Everything has been successfully saved as part of the meal.";
    }
}
