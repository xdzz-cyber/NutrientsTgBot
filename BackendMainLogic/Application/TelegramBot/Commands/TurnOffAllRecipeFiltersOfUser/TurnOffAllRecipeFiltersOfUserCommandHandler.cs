using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.TurnOffAllRecipeFiltersOfUser;

public class TurnOffAllRecipeFiltersOfUserCommandHandler : IRequestHandler<TurnOffAllRecipeFiltersOfUserCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public TurnOffAllRecipeFiltersOfUserCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(TurnOffAllRecipeFiltersOfUserCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }

        var recipeFilters = await _ctx.RecipeFiltersUsers
            .Where(rfu => rfu.AppUserId == userInfo.Id && rfu.IsTurnedIn).ToListAsync(cancellationToken);

        foreach (var recipeFilter in recipeFilters)
        {
            recipeFilter.IsTurnedIn = false;
        }

        await _ctx.SaveChangesAsync(cancellationToken);

        return "All recipe filters have been turned off.";
    }
}
