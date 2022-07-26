using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.ClearRecipesAsPartOfMeal;

public class ClearRecipesAsPartOfMealCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public ClearRecipesAsPartOfMealCommand(string username, long chatId) => (Username, ChatId) = (username, chatId);
}
