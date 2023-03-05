using System.Text;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetRecipesAsPartOfMeal;

public class GetRecipesAsPartOfMealQueryHandler : IRequestHandler<GetRecipesAsPartOfMealQuery,List<Recipe>>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public GetRecipesAsPartOfMealQueryHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<List<Recipe>> Handle(GetRecipesAsPartOfMealQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        var recipes = await _ctx.Recipes.Where(recipe => _ctx.RecipesUsers
            .Where(ru => ru.AppUserId == userInfo.Id)
            .Any(r => r.RecipeId == recipe.Id && r.IsPartOfTheMeal)).ToListAsync(cancellationToken);

        return recipes;
    }
}
