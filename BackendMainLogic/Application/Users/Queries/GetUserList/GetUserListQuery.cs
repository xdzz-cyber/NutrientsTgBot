using Application.Users.Queries.GetUserDetails;
using MediatR;

namespace Application.Users.Queries.GetUserList;

public class GetUserListQuery : IRequest<UserListVm>, IRequest<UserDetailsVm>
{
    public Guid Id { get; set; }
    public string Username { get; set; }
}
