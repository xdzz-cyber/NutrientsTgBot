using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Queries.FindUser;

public class FindUserQueryHandler : IRequestHandler<FindUserQuery, AppUser>
{
    private readonly UserManager<AppUser> _userManager;

    public FindUserQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<AppUser> Handle(FindUserQuery request, CancellationToken cancellationToken)
    {
        var foundUser = await _userManager.FindByNameAsync(request.Username);

        return foundUser ?? null;
    }
}
