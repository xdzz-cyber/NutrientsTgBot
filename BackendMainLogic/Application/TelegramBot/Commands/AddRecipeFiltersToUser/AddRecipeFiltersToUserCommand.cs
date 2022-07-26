using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.AddRecipeFiltersToUser;

public class AddRecipeFiltersToUserCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public string Filters { get; set; }

    public AddRecipeFiltersToUserCommand(string username, long chatId, string filters) 
        => (Username, ChatId, Filters) = (username, chatId, filters);
}
