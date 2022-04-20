using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IGetHashCodeOfString _getSha256CodeOfString;


    public CreateUserCommandHandler(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IGetHashCodeOfString getSha256CodeOfString)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _getSha256CodeOfString = getSha256CodeOfString;
    }
    
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        request.Password = _getSha256CodeOfString.GetHashCodeOfString(request.Password);
        
        var userRole = new IdentityRole("User");
        var newUser = new IdentityUser { UserName = request.Usesrname, PasswordHash = request.Password,Email = request.Email, PhoneNumber = request.Phone};
        
        await _roleManager.CreateAsync(userRole);
        await _userManager.CreateAsync(newUser, request.Password);
        await _userManager.AddToRoleAsync(newUser, userRole.Name);

        return Guid.Parse(newUser.Id);
    }
}
