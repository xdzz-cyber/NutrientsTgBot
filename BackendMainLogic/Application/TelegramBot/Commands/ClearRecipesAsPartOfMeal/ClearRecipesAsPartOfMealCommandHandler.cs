using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.ClearRecipesAsPartOfMeal;

public class ClearRecipesAsPartOfMealCommandHandler : IRequestHandler<ClearRecipesAsPartOfMealCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public ClearRecipesAsPartOfMealCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(ClearRecipesAsPartOfMealCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }

        var likedRecipesByCurrentUser = await _ctx.RecipesUsers
            .Where(ru => ru.AppUserId == userInfo.Id).ToListAsync(cancellationToken: cancellationToken);

        foreach (var recipe in likedRecipesByCurrentUser)
        {
            if (recipe.IsPartOfTheMeal)
            {
                recipe.IsPartOfTheMeal = false;
            }
        }

        await _ctx.SaveChangesAsync(cancellationToken);

        return "All recipes have been cleared as part of the meal.";
    }
}
