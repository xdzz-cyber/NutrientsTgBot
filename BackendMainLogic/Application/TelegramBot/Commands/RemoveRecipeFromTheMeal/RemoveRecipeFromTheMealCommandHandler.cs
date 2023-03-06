﻿using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.RemoveRecipeFromTheMeal;

public class RemoveRecipeFromTheMealCommandHandler : IRequestHandler<RemoveRecipeFromTheMealCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;

    public RemoveRecipeFromTheMealCommandHandler(ITelegramBotDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<string> Handle(RemoveRecipeFromTheMealCommand request, CancellationToken cancellationToken)
    {
        var recipe = await _ctx.RecipesUsers
            .FirstOrDefaultAsync(ru => ru.RecipeId.ToString() == request.RecipeId, cancellationToken);

        if (recipe is null)
        {
            return "Recipe doesn't exist.";
        }

        if (!recipe.IsPartOfTheMeal)
        {
            return "Recipe is not part of the meal.";
        }

        recipe.IsPartOfTheMeal = false;
        
        await _ctx.SaveChangesAsync(cancellationToken);

        return "Recipe has been successfully removed as part of the meal.";
    }
}
