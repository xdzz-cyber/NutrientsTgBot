using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.TurnOffAllRecipeFiltersOfUser;

public class TurnOffAllRecipeFiltersOfUserCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    public long ChatId { get; set; }

    public TurnOffAllRecipeFiltersOfUserCommand(string username, long chatId) => (Username, ChatId) = (username, chatId);
}
