using Application.Common.Constants;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public CreateUserCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        if (await _userManager.FindByNameAsync(request.Username) is not null)
        {
            return Guid.Parse(_userManager.FindByNameAsync(request.Username).Result.Id);
        }
        
        //request.Password = _getSha256CodeOfString.GetHashCodeOfString(request.Password);
        
        if (!_roleManager.Roles.Any(x => x.Name.Equals(AuthRoles.User.ToString())))
        {
            var userRole = new IdentityRole(AuthRoles.User.ToString());

            await _roleManager.CreateAsync(userRole);
        }
        
        var newUser = new AppUser { UserName = request.Username, Age = request.Age,Email = "", 
            PhoneNumber = "", ChatId = 0,  Sex = "", Height = 0};

        await _userManager.CreateAsync(newUser, request.Password);

        var roleName = await _roleManager.FindByNameAsync(AuthRoles.User.ToString());
        
        await _userManager.AddToRoleAsync(newUser, roleName.Name);

        return Guid.Parse(newUser.Id);
    }
}
