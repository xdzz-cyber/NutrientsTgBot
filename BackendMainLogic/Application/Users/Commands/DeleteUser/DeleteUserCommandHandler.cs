using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{

    private readonly UserManager<IdentityUser> _userManager;

    public DeleteUserCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
        {
            throw new NotFoundException(nameof(IdentityUser), request.Id);
        }
        
        await _userManager.DeleteAsync(user);
        
        return Unit.Value;
    }
}
