using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.AddAllRecipesAsPartOfMeal;

public class AddAllRecipesAsPartOfMealCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public AddAllRecipesAsPartOfMealCommand(string username, long chatId)
        => (Username, ChatId) = (username, chatId);
}
