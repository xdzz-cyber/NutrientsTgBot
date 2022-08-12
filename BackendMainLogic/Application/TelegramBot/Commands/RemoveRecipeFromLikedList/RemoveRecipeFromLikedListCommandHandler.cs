using System.Text.RegularExpressions;
using Application.Common.Constants;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.RemoveRecipeFromLikedList;

public class RemoveRecipeFromLikedListCommandHandler : IRequestHandler<RemoveRecipeFromLikedListCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;

    public RemoveRecipeFromLikedListCommandHandler(ITelegramBotDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<string> Handle(RemoveRecipeFromLikedListCommand request, CancellationToken cancellationToken)
    {
        var matchPartOfInputData = Regex.Matches(request.RecipeId, TelegramBotRecipeCommandsNQueriesDataPatterns.InputDataPatternForSingleId);

        var recipeId = string.Join("", matchPartOfInputData);
        
        var recipe = await _ctx.RecipesUsers
            .FirstOrDefaultAsync(ru => ru.RecipeId.ToString() == recipeId, cancellationToken: cancellationToken);

        if (recipe is null)
        {
            return "Recipe is not part of the liked list.";
        }

        _ctx.RecipesUsers.Remove(recipe);

        await _ctx.SaveChangesAsync(cancellationToken);

        return "Recipe has been successfully remove from the liked list.";
    }
}
