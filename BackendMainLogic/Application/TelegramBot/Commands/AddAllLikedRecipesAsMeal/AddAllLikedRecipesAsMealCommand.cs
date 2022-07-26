using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.AddAllLikedRecipesAsMeal;

public class AddAllLikedRecipesAsMealCommand : IRequest<string>,ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public AddAllLikedRecipesAsMealCommand(string username, long chatId) => (Username, ChatId) = (username, chatId);
}
