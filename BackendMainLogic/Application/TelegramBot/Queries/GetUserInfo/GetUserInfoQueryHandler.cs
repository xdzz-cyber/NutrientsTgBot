using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Queries.GetUserInfo;

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, string>
{
    private readonly UserManager<AppUser> _userManager;

    public GetUserInfoQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<string> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user.Weight <= 0 || user.Height <= 0 || user.Age <= 0 || (user.Sex.ToLower() != "man" && user.Sex.ToLower() != "women"))
        {
            return "Please, set all of the values (height, age, weight and sex).";
        }

        return $"Your tallness is {user.Height}cm, age is {user.Age}, weight is {user.Weight}kg and sex is {user.Sex.ToLower()}";
    }
}
