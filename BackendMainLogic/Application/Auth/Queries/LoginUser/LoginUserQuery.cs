using Domain.Auth;
using MediatR;

namespace Application.Auth.Queries.LoginUser;

public class LoginUserQuery :  IRequest<bool>
{
    public string Username { get; set; } = null!;
    
    public string Password { get; set; } = null!;
}
