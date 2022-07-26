using System.Text;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetRecipesAsPartOfMeal;

public class GetRecipesAsPartOfMealQueryHandler : IRequestHandler<GetRecipesAsPartOfMealQuery,string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public GetRecipesAsPartOfMealQueryHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(GetRecipesAsPartOfMealQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        // var recipesAsPartOfMeal = await _ctx.RecipesUsers
        //     .Where(ru => ru.AppUserId == userInfo.Id).ToListAsync(cancellationToken);
        
        var recipes = await _ctx.Recipes.Where(recipe => _ctx.RecipesUsers
            .Where(ru => ru.AppUserId == userInfo.Id).Any(r => r.RecipeId == recipe.Id && r.IsPartOfTheMeal)).ToListAsync(cancellationToken);
        
        var response = new StringBuilder();
        
        foreach (var recipe in recipes)
        {
            var msg = $"<strong>Title: {recipe.Title}, Vegetarian: {recipe.Vegetarian}, GlutenFree: {recipe.GlutenFree}, " + 
                      $"PricePerServing: {recipe.PricePerServing}, Link: {recipe.SpoonacularSourceUrl}</strong>";
            response.AppendLine(msg);
        }

        return string.IsNullOrEmpty(response.ToString()) ? "No recipes found" : response.ToString();
    }
}
