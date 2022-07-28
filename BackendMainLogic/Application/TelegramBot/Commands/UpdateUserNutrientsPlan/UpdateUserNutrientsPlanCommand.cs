using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateUserNutrientsPlan;

public class UpdateUserNutrientsPlanCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string Nutrients { get; set; }

    public UpdateUserNutrientsPlanCommand(string username, long chatId, string nutrients)
        => (Username, ChatId, Nutrients) = (username, chatId, nutrients);
}
