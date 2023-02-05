using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.AddRecipesToUser;

public class AddRecipesToUserCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public List<string> RecipeIds { get; set; }

    public AddRecipesToUserCommand(string username, List<string> recipeIds, long chatId = 0)
        => (Username, RecipeIds, ChatId) = (username, recipeIds, chatId);
}
