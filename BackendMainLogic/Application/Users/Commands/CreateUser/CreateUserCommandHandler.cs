using System.Security.Cryptography;
using System.Text;
using Application.Common.Constants;
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
        
        if (!_roleManager.Roles.Any(x => x.Name.Equals(AuthRoles.User.ToString())))
        {
            var userRole = new IdentityRole(AuthRoles.User.ToString());

            await _roleManager.CreateAsync(userRole);
        }
        
        var newUser = new IdentityUser { UserName = request.Usesrname, Email = request.Email, PhoneNumber = request.Phone};
        await _userManager.CreateAsync(newUser, request.Password);

        var roleName = await _roleManager.FindByNameAsync(AuthRoles.User.ToString());
        await _userManager.AddToRoleAsync(newUser, roleName.Name);

        return Guid.Parse(newUser.Id);
    }
}
