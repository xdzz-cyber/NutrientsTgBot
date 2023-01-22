using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateUserAge;

public class UpdateUserAgeCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string Age { get; set; }

    public UpdateUserAgeCommand(string username, string age ,long chatId = 0)
        => (Username, Age, ChatId) = (username, age, chatId);
}
