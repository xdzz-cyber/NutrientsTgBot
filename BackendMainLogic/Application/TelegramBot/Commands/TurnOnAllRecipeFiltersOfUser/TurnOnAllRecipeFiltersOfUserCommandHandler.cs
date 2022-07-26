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

        var recipeFilters = await _ctx.RecipeFiltersUsers
            .Where(rfu => rfu.AppUserId == userInfo.Id && !rfu.IsTurnedIn).ToListAsync(cancellationToken);

        foreach (var recipeFilter in recipeFilters)
        {
            recipeFilter.IsTurnedIn = true;
        }

        await _ctx.SaveChangesAsync(cancellationToken);

        return "All recipe filters have been turned off.";
    }
}
