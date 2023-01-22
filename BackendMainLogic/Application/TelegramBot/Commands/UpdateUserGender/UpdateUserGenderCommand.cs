using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateUserGender;

public class UpdateUserGenderCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string Sex { get; set; }

    public UpdateUserGenderCommand(string username, string sex, long chatId = 0)
        => (Username, Sex, ChatId) = (username, sex, chatId);
}
