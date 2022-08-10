using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.AddAllLikedRecipesAsMeal;

public class AddAllLikedRecipesAsMealCommandHandler : IRequestHandler<AddAllLikedRecipesAsMealCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public AddAllLikedRecipesAsMealCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(AddAllLikedRecipesAsMealCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }

        var likedRecipes = await _ctx.RecipesUsers
            .Where(ru => ru.AppUserId == userInfo.Id).ToListAsync(cancellationToken);

        if (!likedRecipes.Any())
        {
            return "No liked recipes found.";
        }

        foreach (var likedRecipe in likedRecipes)
        {
            if (!likedRecipe.IsPartOfTheMeal)
            {
                likedRecipe.IsPartOfTheMeal = true;
            }
        }

        await _ctx.SaveChangesAsync(cancellationToken);

        return "All liked recipes have been successfully added as part of the meal.";
    }
}
