using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetUserFiltersForRecipes;

public class GetUserFiltersForRecipesQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserFiltersForRecipesQuery(string username, long chatId) => (Username, ChatId) = (username, chatId);
}
