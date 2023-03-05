using System.Text;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetUserRecipeList;

public class GetUserRecipeListQueryHandler : IRequestHandler<GetUserRecipeListQuery, List<Recipe>>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public GetUserRecipeListQueryHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<List<Recipe>> Handle(GetUserRecipeListQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        var recipes = await _ctx.Recipes.Where(recipe => _ctx.RecipesUsers
            .Where(ru => ru.AppUserId == userInfo.Id)
            .Any(r => r.RecipeId == recipe.Id)).ToListAsync(cancellationToken);

        return recipes;
    }
}
