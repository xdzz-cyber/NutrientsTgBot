using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.NotifyUserToSetNewWeight;

public class NotifyUserToSetNewWeightQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }
    

    public NotifyUserToSetNewWeightQuery(string username, long chatId) => (Username, ChatId) = (username, chatId);
}
