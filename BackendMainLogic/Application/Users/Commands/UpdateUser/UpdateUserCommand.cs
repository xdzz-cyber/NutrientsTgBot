using MediatR;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string Phone { get; set; }
}
