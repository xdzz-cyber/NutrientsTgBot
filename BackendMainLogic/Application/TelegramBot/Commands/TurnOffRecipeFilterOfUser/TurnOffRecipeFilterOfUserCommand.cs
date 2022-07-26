using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.TurnOffRecipeFilterOfUser;

public class TurnOffRecipeFilterOfUserCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string RecipeFilterId { get; set; }

    public TurnOffRecipeFilterOfUserCommand(string username, long chatId, string recipeFilterId)
        => (Username, ChatId, RecipeFilterId) = (username, chatId, recipeFilterId);
}
