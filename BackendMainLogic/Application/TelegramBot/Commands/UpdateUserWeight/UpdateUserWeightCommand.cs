using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateUserWeight;

public class UpdateAppUserWeightCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public string Weight { get; set; }

    public UpdateAppUserWeightCommand(string username, long chatId, string weight = "")
    => (Username, ChatId, Weight) = (username, chatId, weight);
}
