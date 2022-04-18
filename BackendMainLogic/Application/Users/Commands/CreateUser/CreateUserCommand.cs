using MediatR;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<Guid>
{
    public string Usesrname { get; set; }
}
