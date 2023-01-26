using System.Text;
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

        //var response = new StringBuilder();

        //response.Append("Filters available are: ");

        var existingFilters = recipeFilters.Select(recipe => new UserRecipesFiltersViewDto
        {
            Id = recipe.Id,
            Name = recipe.Name
        }).ToList();

        return existingFilters; //string.Join(",", existingFilters);

        // response.Append(string.Join(",", existingFilters));
        //
        // response.AppendLine($"\n\nIf you want to add a new one, please, click here(/AddRecipeFiltersToUser)");
        //
        // response.AppendLine("Turn on all filters(/TurnOnAllRecipeFiltersOfUser)");
        //
        // if (!recipeFilters.Any())
        // {
        //     response.AppendLine("\nYou have no filters selected");
        //     return response.ToString();
        // }
        //
        // response.AppendLine("\nYour filters:");
        //
        // foreach (var recipeFilter in recipeFilters)
        // {
        //     response.Append($"{recipeFilter.Name} - ");
        //     response.AppendLine($"Turn off filter(/TurnOffRecipeFilterOfUser_{recipeFilter.Id})");
        // }
        //
        // response.AppendLine("\nClear all filters(/TurnOffAllRecipeFiltersOfUser)");
        //
        // return response.ToString();
    }
}
