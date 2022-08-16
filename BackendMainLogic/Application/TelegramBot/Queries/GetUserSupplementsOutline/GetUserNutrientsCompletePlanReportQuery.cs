using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetUserSupplementsOutline;

public class GetUserSupplementsOutlineQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserSupplementsOutlineQuery(string username, long chatId)
        => (Username, ChatId) = (username, chatId);
}
