using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateUserAge;

public class UpdateUserAgeCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string Age { get; set; }

    public UpdateUserAgeCommand(string username, long chatId, string age)
        => (Username, ChatId, Age) = (username, chatId, age);
}
