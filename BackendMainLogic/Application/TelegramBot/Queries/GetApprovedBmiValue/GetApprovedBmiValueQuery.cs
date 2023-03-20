using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetApprovedBmiValue;

public class GetApprovedBmiValueQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetApprovedBmiValueQuery(string username, long chatId = 0)
        => (Username, ChatId) = (username, chatId);
}
