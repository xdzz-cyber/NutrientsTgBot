using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Commands.AddRecipeToUser;

public class AddRecipeToUserCommandHandler : IRequestHandler<AddRecipeToUserCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;
    private readonly HttpClient _httpClient;

    public AddRecipeToUserCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager,
        HttpClient httpClient)
    {
        _ctx = ctx;
        _userManager = userManager;
        _httpClient = httpClient; 
    }
    public async Task<string> Handle(AddRecipeToUserCommand request, CancellationToken cancellationToken)
    {

        try
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (_ctx.RecipesUsers.Count(ru => ru.AppUserId == user.Id) == TelegramBotRecipesPerUserAmount
                    .MaxRecipesPerUser)
            {
                return $"Max limit of saved recipes is exceeded({TelegramBotRecipesPerUserAmount.MaxRecipesPerUser} at most).";
            }

            if (_ctx.RecipesUsers.Any(recipesUsers => recipesUsers.RecipeId.ToString() ==  request.RecipeId 
                                                      && recipesUsers.AppUserId == user.Id))
            {
                return "The chosen recipe has already been added.";
            }

            if (_ctx.Recipes.FirstOrDefault(recipe => recipe.Id == int.Parse(request.RecipeId, NumberStyles.Integer)) ==
                null)
            {
                var rawNotSavedInDbRecipe = await _httpClient
                    .GetAsync(TelegramBotRecipesHttpPaths.GetRecipeById.Replace("id",request.RecipeId), cancellationToken);

                var notSavedInDbRecipe = JsonSerializer.Deserialize<Recipe>(await rawNotSavedInDbRecipe.Content.ReadAsStringAsync(cancellationToken));
                    
                    await _ctx.Recipes.AddAsync(new Recipe
                    {
                        Id = notSavedInDbRecipe!.Id,
                        AggregateLikes = notSavedInDbRecipe.AggregateLikes,
                        CookingMinutes = notSavedInDbRecipe.CookingMinutes,
                        GlutenFree = notSavedInDbRecipe.GlutenFree,
                        HealthScore = notSavedInDbRecipe.HealthScore,
                        PricePerServing = notSavedInDbRecipe.PricePerServing,
                        SourceName = notSavedInDbRecipe.SourceName,
                        SpoonacularSourceUrl = notSavedInDbRecipe.SpoonacularSourceUrl,
                        Title = notSavedInDbRecipe.Title,
                        Vegetarian = notSavedInDbRecipe.Vegetarian
                    }, cancellationToken);
            }

            if (_ctx.RecipesUsers.Count() < TelegramBotRecipesPerUserAmount.MaxRecipesPerUser)
            {
                await _ctx.RecipesUsers.AddAsync(
                    new RecipesUsers
                    {
                        AppUserId = user.Id,
                        RecipeId = int.Parse(request.RecipeId, NumberStyles.Integer)
                    }, cancellationToken); 
            }
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return "Inner server error.";
        }

        return "New recipe has been added successfully";
    }
}
