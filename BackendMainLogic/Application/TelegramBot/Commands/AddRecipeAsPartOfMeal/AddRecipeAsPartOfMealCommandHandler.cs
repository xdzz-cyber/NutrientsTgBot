using System.Text.RegularExpressions;
using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.AddRecipeAsPartOfMeal;

public class AddRecipeAsPartOfMealCommandHandler : IRequestHandler<AddRecipeAsPartOfMealCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public AddRecipeAsPartOfMealCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }

    public async Task<string> Handle(AddRecipeAsPartOfMealCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }
        
        var matchPartOfInputData = Regex.Matches(request.RecipeId, TelegramBotRecipeCommandsNQueriesDataPatterns.InputDataPatternForSingleId);

        var recipeId = string.Join("", matchPartOfInputData);

        var recipe = await _ctx.RecipesUsers
            .FirstOrDefaultAsync(ru => ru.RecipeId.ToString() == recipeId, cancellationToken: cancellationToken);

        if (recipe is null && _ctx.RecipesUsers.Count() 
            < TelegramBotRecipesPerUserAmount.MaxRecipesPerUser)
        {
            await _ctx.RecipesUsers.AddAsync(new RecipesUsers
            {
                AppUserId = userInfo.Id,
                IsPartOfTheMeal = true,
                RecipeId = Convert.ToInt32(recipeId)
            }, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            //return "Recipe doesn't exist.";
        }
        else if (recipe is not null && recipe.IsPartOfTheMeal)
        {
            return "Recipe has already been added as part of the meal.";
        }
        else if (recipe is not null)
        {
            recipe.IsPartOfTheMeal = true;
        }

        
        await _ctx.SaveChangesAsync(cancellationToken);

        return "Recipe has been successfully added as part of the meal";
    }
}
