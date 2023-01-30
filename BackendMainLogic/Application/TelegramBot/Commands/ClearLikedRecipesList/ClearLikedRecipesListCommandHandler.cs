using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.ClearLikedRecipesList;

public class ClearLikedRecipesListCommandHandler : IRequestHandler<ClearLikedRecipesListCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public ClearLikedRecipesListCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }

    public async Task<string> Handle(ClearLikedRecipesListCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        var likedRecipes = await _ctx.RecipesUsers
            .Where(ru => ru.AppUserId == userInfo.Id).ToListAsync(cancellationToken);
        
        _ctx.RecipesUsers.RemoveRange(likedRecipes);
        
        await _ctx.SaveChangesAsync(cancellationToken);

        return "Liked recipes have been successfully deleted.";
    }
}
