using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.TurnOnAllRecipeFiltersOfUser;

public class TurnOnAllRecipeFiltersOfUserCommandHandler : IRequestHandler<TurnOnAllRecipeFiltersOfUserCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public TurnOnAllRecipeFiltersOfUserCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(TurnOnAllRecipeFiltersOfUserCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }
        
        var recipeFiltersOfUser = await _ctx.RecipeFiltersUsers
            .Where(rfu => rfu.AppUserId == userInfo.Id).ToListAsync(cancellationToken); //&& !rfu.IsTurnedIn

        if (recipeFiltersOfUser.All(rf => rf.IsTurnedIn) 
            && recipeFiltersOfUser.Count == TelegramBotRecipeFilters.RecipeFilters.Count)
        {
            return "All filters have already been saved.";
        }

        if (!recipeFiltersOfUser.Any())
        {
            var recipeFiltersNames = TelegramBotRecipeFilters.RecipeFilters;

            foreach (var recipeFilterName in recipeFiltersNames)
            {
                await _ctx.RecipeFiltersUsers.AddAsync(new RecipeFiltersUsers
                {
                    AppUserId = userInfo.Id,
                    IsTurnedIn = true,
                    RecipeFiltersId = _ctx.RecipeFilters.First(rf => rf.Name.Equals(recipeFilterName)).Id 
                }, cancellationToken);
            }
        }
        else
        {
            foreach (var recipeFilter in recipeFiltersOfUser)
            {
                recipeFilter.IsTurnedIn = true;
            }
        }

        await _ctx.SaveChangesAsync(cancellationToken);

        return "All recipe filters have been turned on.";
    }
}
