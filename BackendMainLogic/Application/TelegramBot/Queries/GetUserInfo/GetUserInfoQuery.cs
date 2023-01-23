using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetUserInfo;

public class GetUserInfoQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserInfoQuery(string username, long chatId = 0)
        => (Username, ChatId) = (username, chatId);
}
