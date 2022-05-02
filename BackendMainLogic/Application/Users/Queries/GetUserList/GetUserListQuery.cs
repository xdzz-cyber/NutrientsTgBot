using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries.GetUserList;

public class GetUserListQuery : IRequest<UserListVm>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserListQuery(string username = "", long chatId = 0) => (Username, ChatId) = (username, chatId);
}
