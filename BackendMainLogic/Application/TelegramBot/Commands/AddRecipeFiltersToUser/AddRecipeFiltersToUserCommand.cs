using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.AddRecipeFiltersToUser;

public class AddRecipeFiltersToUserCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public string Filters { get; set; }

    public AddRecipeFiltersToUserCommand(string username, string filters ,long chatId = 0) 
        => (Username, Filters, ChatId) = (username, filters, chatId);
}
