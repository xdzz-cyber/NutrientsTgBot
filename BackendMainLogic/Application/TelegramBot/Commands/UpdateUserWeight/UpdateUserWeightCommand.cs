using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateUserWeight;

public class UpdateAppUserWeightCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public string Weight { get; set; }

    public UpdateAppUserWeightCommand(string username, string weight = "" , long chatId = 0)
    => (Username, Weight, ChatId) = (username, weight, chatId);
}
