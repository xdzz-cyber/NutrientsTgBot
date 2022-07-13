using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.AddRecipeToUser;

public class AddRecipeToUserCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public string RecipeId { get; set; }

    public AddRecipeToUserCommand(string username, long chatId, string recipeId)
        => (Username, ChatId, RecipeId) = (username, chatId, recipeId);
}
