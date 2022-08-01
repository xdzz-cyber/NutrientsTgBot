using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.AddRecipesAsPartOfMeal;

public class AddRecipesAsPartOfMealCommandHandler : IRequestHandler<AddRecipesAsPartOfMealCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public AddRecipesAsPartOfMealCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(AddRecipesAsPartOfMealCommand request, CancellationToken cancellationToken)
    {
        var mealsIds = StateManagement.TempData["MealsIds"].Split(',');

        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        foreach (var mealId in mealsIds)
        {
            var meal = await _ctx.RecipesUsers
                .FirstOrDefaultAsync(ru => ru.RecipeId.ToString() == mealId && ru.AppUserId == userInfo.Id, cancellationToken);

            meal!.IsPartOfTheMeal = true;
        }

        await _ctx.SaveChangesAsync(cancellationToken);

        return "Everything has been successfully saved as part of the meal.";
    }
}
