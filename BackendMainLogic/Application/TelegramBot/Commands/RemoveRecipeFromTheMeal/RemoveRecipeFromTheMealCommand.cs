using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.RemoveRecipeFromTheMeal;

public class RemoveRecipeFromTheMealCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string RecipeId { get; set; }
    
    public RemoveRecipeFromTheMealCommand(string username, long chatId, string recipeId)
        => (Username, ChatId, RecipeId) = (username, chatId, recipeId);
}
