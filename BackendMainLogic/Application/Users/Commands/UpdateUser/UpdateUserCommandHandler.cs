using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IGetHashCodeOfString _getSha256CodeOfString;

    public UpdateUserCommandHandler(UserManager<IdentityUser> userManager, IGetHashCodeOfString getSha256CodeOfString)
    {
        _userManager = userManager;
        _getSha256CodeOfString = getSha256CodeOfString;
    }
    
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
       
       if (user == null)
       {
           throw new NotFoundException(nameof(IdentityUser), request.Id);
       }

        user.UserName = request.Username;
        user.PasswordHash = _getSha256CodeOfString.GetHashCodeOfString(request.Password);
        user.Email = request.Email;
        user.PhoneNumber = request.Phone;
        
        await _userManager.UpdateAsync(user);
        
        return Unit.Value;
    }
}
