using Application.Common.Exceptions;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly ITelegramBotDbContext _ctx;

    public UpdateUserCommandHandler(ITelegramBotDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        user.Username = request.Username;

        await _ctx.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
