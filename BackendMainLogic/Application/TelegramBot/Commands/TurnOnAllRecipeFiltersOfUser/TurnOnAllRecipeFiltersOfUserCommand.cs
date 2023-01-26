using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.TurnOnAllRecipeFiltersOfUser;

public class TurnOnAllRecipeFiltersOfUserCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public TurnOnAllRecipeFiltersOfUserCommand(string username, long chatId = 0)
        => (Username, ChatId) = (username, chatId);
}
