using Application.Common.Exceptions;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{

    private readonly UserManager<AppUser> _userManager;

    public DeleteUserCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            throw new NotFoundException(nameof(AppUser), request.Username);
        }
        
        await _userManager.DeleteAsync(user);
        
        return Unit.Value;
    }
}
