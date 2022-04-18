using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly ITelegramBotDbContext _ctx;

    public CreateUserCommandHandler(ITelegramBotDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Username = request.Usesrname
        };

        await _ctx.Users.AddAsync(user, cancellationToken);
        await _ctx.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
