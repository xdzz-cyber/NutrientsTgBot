using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetRecipesAsPartOfMeal;

public class GetRecipesAsPartOfMealQuery : IRequest<string>,IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetRecipesAsPartOfMealQuery(string username, long chatId) => (Username, ChatId) = (username, chatId);
}
