using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateUserGender;

public class UpdateUserGenderCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string Sex { get; set; }

    public UpdateUserGenderCommand(string username, long chatId, string sex)
        => (Username, ChatId, Sex) = (username, chatId, sex);
}
