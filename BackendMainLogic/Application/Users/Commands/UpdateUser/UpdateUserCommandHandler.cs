using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IGetHashCodeOfString _getSha256CodeOfString;

    public UpdateUserCommandHandler(UserManager<AppUser> userManager, IGetHashCodeOfString getSha256CodeOfString)
    {
        _userManager = userManager;
        _getSha256CodeOfString = getSha256CodeOfString;
    }
    
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
       
       if (user == null)
       {
           throw new NotFoundException(nameof(AppUser), request.Username);
       }

       user.UserName = request.Username;
        user.PasswordHash = _getSha256CodeOfString.GetHashCodeOfString(request.Password);
        user.Email = request.Email;
        user.PhoneNumber = request.Phone;
        user.ChatId = request.ChatId;

        await _userManager.UpdateAsync(user);
        
        return Unit.Value;
    }
}
