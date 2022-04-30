using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Queries.LoginUser;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, bool>
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginUserQueryHandler(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    public async Task<bool> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);
        return  result.Succeeded;
    }
}
