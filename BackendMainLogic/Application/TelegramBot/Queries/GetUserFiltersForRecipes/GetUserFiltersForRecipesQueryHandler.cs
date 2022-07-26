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
        
        // var recipeFiltersChosenByUser = await _ctx.RecipeFiltersUsers
        //     .Where(rfu => rfu.AppUserId == userInfo.Id).ToListAsync(cancellationToken);
        
        var recipeFilters = await _ctx.RecipeFilters
            .Where(rf => _ctx.RecipeFiltersUsers.Any(rfu => rfu.RecipeFiltersId == rf.Id && rfu.AppUserId == userInfo.Id && rfu.IsTurnedIn))
            .ToListAsync(cancellationToken);

        var response = new StringBuilder();
        
        response.AppendLine("Filters available go below:");
        
        foreach (var recipe in _ctx.RecipeFilters)
        {
            response.AppendLine($"{recipe.Name}");
        }

        response.AppendLine($"If you want to add a new one, please, click here(/AddRecipeFiltersToUser)");
        response.AppendLine("Turn on all filters(/TurnOnAllRecipeFiltersOfUser)");
        
        if (!recipeFilters.Any())
        {
            response.AppendLine("You have no filters selected.");
            return response.ToString();
        }

        response.AppendLine("Your filters:");

        foreach (var recipeFilter in recipeFilters)
        {
            response.AppendLine($"<strong>{recipeFilter.Name}</strong>");
            response.AppendLine($"<strong>Turn off filter(/TurnOffRecipeFilterOfUser_{recipeFilter.Id})</strong>");
        }

        response.AppendLine("Clear all filters(/TurnOffAllRecipeFiltersOfUser)");
        

        return response.ToString();
    }
}
