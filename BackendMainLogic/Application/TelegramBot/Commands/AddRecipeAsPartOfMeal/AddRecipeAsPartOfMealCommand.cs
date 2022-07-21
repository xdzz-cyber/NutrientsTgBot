using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Commands.AddRecipeAsPartOfMeal;

public class AddRecipeAsPartOfMealCommand : IRequest<string>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public string RecipeId { get; set; }

    public AddRecipeAsPartOfMealCommand(string username, long chatId, string recipeId)
        => (Username, ChatId, RecipeId) = (username, chatId, recipeId);
}
