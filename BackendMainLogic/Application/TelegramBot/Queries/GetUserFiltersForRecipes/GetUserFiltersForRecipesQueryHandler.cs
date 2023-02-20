using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using Domain.TelegramBotEntities.RecipesFilters;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetUserFiltersForRecipes;

public class GetUserFiltersForRecipesQueryHandler : IRequestHandler<GetUserFiltersForRecipesQuery, IList<UserRecipesFiltersViewDto>>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public GetUserFiltersForRecipesQueryHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<IList<UserRecipesFiltersViewDto>> Handle(GetUserFiltersForRecipesQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        if (!_ctx.RecipeFilters.Any())
        {
            var newRecipeFilters = TelegramBotRecipeFilters.RecipeFilters
                .Select(recipeFilter => new RecipeFilters {Name = recipeFilter}).ToList();

            await _ctx.RecipeFilters.AddRangeAsync(newRecipeFilters, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        var recipeFilters = await _ctx.RecipeFilters
            .Where(rf => _ctx.RecipeFiltersUsers.Any(rfu => rfu.RecipeFiltersId == rf.Id 
                                                            && rfu.AppUserId == userInfo.Id && rfu.IsTurnedIn))
            .ToListAsync(cancellationToken);

        var existingFilters = recipeFilters.Select(recipe => new UserRecipesFiltersViewDto
        {
            Id = recipe.Id,
            Name = recipe.Name
        }).ToList();

        return existingFilters;
    }
}
