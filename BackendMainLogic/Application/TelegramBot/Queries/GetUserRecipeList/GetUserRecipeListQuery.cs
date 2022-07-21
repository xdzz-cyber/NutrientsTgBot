using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetUserRecipeList;

public class GetUserRecipeListQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserRecipeListQuery(string username, long chatId) => (Username, ChatId) = (username, chatId);
}
