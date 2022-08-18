using System.Text;
using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetUserFiltersForRecipes;

public class GetUserFiltersForRecipesQueryHandler : IRequestHandler<GetUserFiltersForRecipesQuery, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public GetUserFiltersForRecipesQueryHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(GetUserFiltersForRecipesQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }
            
        if (!_ctx.RecipeFilters.Any())
        {
            var newRecipeFilters = new List<RecipeFilters>();
            foreach (var recipeFilter in TelegramBotRecipeFilters.RecipeFilters)
            {
                newRecipeFilters.Add( new RecipeFilters {Name = recipeFilter});
            }

            await _ctx.RecipeFilters.AddRangeAsync(newRecipeFilters, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        var recipeFilters = await _ctx.RecipeFilters
            .Where(rf => _ctx.RecipeFiltersUsers.Any(rfu => rfu.RecipeFiltersId == rf.Id 
                                                            && rfu.AppUserId == userInfo.Id && rfu.IsTurnedIn))
            .ToListAsync(cancellationToken);

        var response = new StringBuilder();

        var existingFilters = new List<string>();
        
        response.Append("Filters available are: ");
        
        foreach (var recipe in _ctx.RecipeFilters)
        {
            existingFilters.Add($"<strong>{recipe.Name}</strong>");
        }

        response.Append(string.Join(",", existingFilters));

        response.AppendLine($"\n\nIf you want to add a new one, please, click here(/AddRecipeFiltersToUser)");
        
        response.AppendLine("Turn on all filters(/TurnOnAllRecipeFiltersOfUser)");
        
        if (!recipeFilters.Any())
        {
            response.AppendLine("\n<strong>You have no filters selected.</strong>");
            return response.ToString();
        }

        response.AppendLine("\nYour filters:");

        foreach (var recipeFilter in recipeFilters)
        {
            response.Append($"<strong>{recipeFilter.Name}</strong> - ");
            response.AppendLine($"Turn off filter(/TurnOffRecipeFilterOfUser_{recipeFilter.Id})");
        }

        response.AppendLine("\nClear all filters(/TurnOffAllRecipeFiltersOfUser)");
        
        return response.ToString();
    }
}
