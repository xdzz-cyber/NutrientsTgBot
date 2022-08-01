using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.AddRecipesAsPartOfMeal;

public class AddRecipesAsPartOfMealCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public AddRecipesAsPartOfMealCommand(string username, long chatId)
        => (Username, ChatId) = (username, chatId);
}
