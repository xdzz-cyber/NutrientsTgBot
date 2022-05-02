using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries;

public class StartApplicationQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    public long ChatId { get; set; }

    public StartApplicationQuery(string username = "", long chatId = 0) => 
        (Username, ChatId) = (username, chatId);
}
