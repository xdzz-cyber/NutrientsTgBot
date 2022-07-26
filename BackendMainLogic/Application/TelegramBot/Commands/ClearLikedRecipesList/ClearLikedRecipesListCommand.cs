using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.ClearLikedRecipesList;

public class ClearLikedRecipesListCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public ClearLikedRecipesListCommand(string username, long chatId) => (Username, ChatId) = (username, chatId);
}
