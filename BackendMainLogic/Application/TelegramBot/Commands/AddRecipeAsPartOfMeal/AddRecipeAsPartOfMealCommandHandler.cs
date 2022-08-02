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
        
        var matchPartOfInputData = Regex.Matches(request.RecipeId, TelegramBotRecipeCommandsNQueriesDataPatterns.InputDataPatternForSingleId);

        var recipe = await _ctx.RecipesUsers
            .FirstOrDefaultAsync(ru => ru.RecipeId.ToString() == matchPartOfInputData.First().Value, cancellationToken: cancellationToken);

        if (recipe is null && _ctx.RecipesUsers.Count(ru => ru.AppUserId == userInfo.Id) 
            <= TelegramBotRecipesPerUserAmount.MaxRecipesPerUser)
        {
            await _ctx.RecipesUsers.AddAsync(new RecipesUsers
            {
                AppUserId = userInfo.Id,
                IsPartOfTheMeal = true,
                RecipeId = Convert.ToInt32(matchPartOfInputData.First().Value)
            }, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            //return "Recipe doesn't exist.";
        }
        else if(_ctx.RecipesUsers.Count(ru => ru.AppUserId == userInfo.Id) 
                == TelegramBotRecipesPerUserAmount.MaxRecipesPerUser)
        {
            return "Limit of saved recipes has been exceeded. Please, remove some to add new ones.";
        }

        if (recipe!.IsPartOfTheMeal)
        {
            return "Recipe has already been added as part of the meal.";
        }

        recipe.IsPartOfTheMeal = true;
        await _ctx.SaveChangesAsync(cancellationToken);

        return "Recipe has been successfully added as part of the meal";
    }
}
