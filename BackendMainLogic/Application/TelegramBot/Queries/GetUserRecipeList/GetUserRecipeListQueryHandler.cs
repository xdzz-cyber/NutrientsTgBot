using System.Text;
using System.Text.Json;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetUserRecipeList;

public class GetUserRecipeListQueryHandler : IRequestHandler<GetUserRecipeListQuery, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public GetUserRecipeListQueryHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(GetUserRecipeListQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        var recipes = await _ctx.Recipes.Where(recipe => _ctx.RecipesUsers
            .Where(ru => ru.AppUserId == userInfo.Id).Any(r => r.RecipeId == recipe.Id)).ToListAsync(cancellationToken);

        if (recipes.Count == 0)
        {
            return "No recipes found.";
        }
        
        var response = new StringBuilder();
        
        foreach (var recipe in recipes)
        {
            var msg = $"<strong>Title: {recipe.Title}, Vegetarian: {recipe.Vegetarian}, GlutenFree: {recipe.GlutenFree}, " + 
                      $"PricePerServing: {recipe.PricePerServing}, Link: {recipe.SpoonacularSourceUrl}</strong>";
            response.AppendLine(msg);
            response.AppendLine($"Save as a part of the meal(/AddRecipeAsPartOfMeal_{recipe.Id})");
            response.AppendLine($"Remove from the meal(/RemoveRecipeFromTheMeal_{recipe.Id})");
            response.AppendLine($"Remove from the liked list(/RemoveRecipeFromLikedList_{recipe.Id})");
        }

        response.AppendLine("Clear liked list(/ClearLikedRecipesList)");
        response.AppendLine("Remove all from meal(/ClearRecipesAsPartOfMeal)");
        response.AppendLine("Add all liked recipes as part of the meal(/AddAllLikedRecipesAsMeal)");
        
        return response.ToString();
    }
}
