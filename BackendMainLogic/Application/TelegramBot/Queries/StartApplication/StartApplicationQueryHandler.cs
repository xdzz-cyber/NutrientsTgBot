using Application.Users.Commands.CreateUser;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Queries.StartApplication;

public class StartApplicationQueryHandler : IRequestHandler<StartApplicationQuery, string>
{
    private readonly IMediator _mediator;
    private readonly UserManager<AppUser> _userManager;

    public StartApplicationQueryHandler(IMediator mediator, UserManager<AppUser> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(StartApplicationQuery request, CancellationToken cancellationToken)
    {
            var user = await _userManager.FindByNameAsync(request.Username);
            
            var userId = Guid.NewGuid();
            
            if (user is null)
            {
                userId = await _mediator.Send(new CreateUserCommand(username: request.Username, chatId: request.ChatId), 
                    cancellationToken);
            }
            
            return string.IsNullOrEmpty(userId.ToString())
                ? "Bad data given. Try to start again" : "You've been successfully logged in.";
    }
}
