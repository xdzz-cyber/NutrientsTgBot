using Application.Interfaces;
using Domain.TelegramBotEntities.NutrientsPlan;
using MediatR;

namespace Application.TelegramBot.Commands.UpdateUserNutrientsPlan;

public class UpdateUserNutrientsPlanCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public NutrientsPlanFormDto Nutrients { get; set; }

    public UpdateUserNutrientsPlanCommand(string username, NutrientsPlanFormDto nutrients, long chatId = 0)
        => (Username, Nutrients, ChatId) = (username, nutrients, chatId);
}
