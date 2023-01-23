using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateUserWaterBalanceLevel;

public class UpdateUserWaterBalanceLevelCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public string AmountOfWater { get; set; }

    public UpdateUserWaterBalanceLevelCommand(string username, string amountOfWater, long chatId = 0)
    => (Username, AmountOfWater, ChatId) = (username, amountOfWater, chatId);
}
