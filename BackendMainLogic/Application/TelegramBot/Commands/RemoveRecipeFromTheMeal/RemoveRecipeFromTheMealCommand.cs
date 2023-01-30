using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.RemoveRecipeFromTheMeal;

public class RemoveRecipeFromTheMealCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string RecipeId { get; set; }
    
    public RemoveRecipeFromTheMealCommand(string username, string recipeId, long chatId = 0)
        => (Username, RecipeId, ChatId) = (username, recipeId, chatId);
}
