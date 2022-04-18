using MediatR;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public Guid Id { get; set; }

    public string Username { get; set; }
}
