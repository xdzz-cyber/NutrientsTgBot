using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.RemoveRecipeFromLikedList;

public class RemoveRecipeFromLikedListCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string RecipeId { get; set; }
    
    public RemoveRecipeFromLikedListCommand(string username, string recipeId, long chatId = 0)
        => (Username, RecipeId, ChatId) = (username, recipeId, chatId);
}
