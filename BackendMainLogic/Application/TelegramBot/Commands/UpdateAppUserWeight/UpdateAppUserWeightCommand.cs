using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateAppUserWeight;

public class UpdateAppUserWeightCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public double Weight { get; set; }

    public UpdateAppUserWeightCommand(string username, long chatId, double weight)
    => (Username, ChatId, Weight) = (username, chatId, weight);
}
