using System.Text.RegularExpressions;
using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.TurnOffRecipeFilterOfUser;

public class TurnOffRecipeFilterOfUserCommandHandler : IRequestHandler<TurnOffRecipeFilterOfUserCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public TurnOffRecipeFilterOfUserCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(TurnOffRecipeFilterOfUserCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        var recipeFilter = await _ctx.RecipeFiltersUsers
            .FirstOrDefaultAsync(rfu => rfu.AppUserId == userInfo.Id 
                               && rfu.RecipeFiltersId.ToString() == request.RecipeFilterId, 
                cancellationToken: cancellationToken);

        if (recipeFilter is null)
        {
            return "Wrong recipe id given";
        }

        recipeFilter.IsTurnedIn = false;

        await _ctx.SaveChangesAsync(cancellationToken);

        return "Chosen recipe filter has been successfully turned off";
    }
}
