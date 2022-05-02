using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Authentication.a;

namespace Application.Auth.Queries.LoginUser;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, bool>
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public LoginUserQueryHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    public async Task<bool> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        //new AuthenticationProperties
        await _signInManager.SignInAsync(user, false);//await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);
        return  true;
    }
}
