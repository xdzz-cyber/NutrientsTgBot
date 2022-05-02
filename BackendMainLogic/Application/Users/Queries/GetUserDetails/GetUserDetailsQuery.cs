using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries.GetUserDetails;

public class GetUserDetailsQuery : IRequest<UserDetailsVm>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserDetailsQuery(string username = "", long chatId = 0) => (Username, ChatId) = (username, chatId);
}
