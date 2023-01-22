using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateUserTallness;

public class UpdateUserTallnessCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string Height { get; set; }

    public UpdateUserTallnessCommand(string username, string height ,long chatId = 0)
        => (Username, Height, ChatId) = (username, height, chatId);
}
