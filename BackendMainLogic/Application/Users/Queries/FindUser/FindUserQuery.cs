using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;

namespace Application.Users.Queries.FindUser;

public class FindUserQuery : IRequest<AppUser?>, IQuery
{
    public string Username { get; set; }

    public long ChatId { get; set; }

    public FindUserQuery(string username, long chatId = 0) => (Username, ChatId) = (username, chatId);
}
