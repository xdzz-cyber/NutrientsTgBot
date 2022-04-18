using Application.Common.Exceptions;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly ITelegramBotDbContext _ctx;

    public DeleteUserCommandHandler(ITelegramBotDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        _ctx.Users.Remove(user);
        await _ctx.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
