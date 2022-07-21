using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.AddRecipeAsPartOfMeal;

public class AddRecipeAsPartOfMealCommandHandler : IRequestHandler<AddRecipeAsPartOfMealCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;

    public AddRecipeAsPartOfMealCommandHandler(ITelegramBotDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<string> Handle(AddRecipeAsPartOfMealCommand request, CancellationToken cancellationToken)
    {
        var recipe = await _ctx.RecipesUsers
            .FirstOrDefaultAsync(ru => ru.RecipeId.ToString() == request.RecipeId, cancellationToken: cancellationToken);

        if (recipe is null)
        {
            return "Recipe doesn't exist.";
        }

        if (recipe.IsPartOfTheMeal)
        {
            return "Recipe has already been added as part of the meal.";
        }

        recipe.IsPartOfTheMeal = true;
        await _ctx.SaveChangesAsync(cancellationToken);

        return "Recipe has been successfully added as part of the meal";
    }
}
