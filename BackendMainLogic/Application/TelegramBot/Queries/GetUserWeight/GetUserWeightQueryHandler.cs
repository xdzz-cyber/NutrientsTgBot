using Application.Common.Constants;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Queries.GetUserWeight;

public class GetUserWeightQueryHandler : IRequestHandler<GetUserWeightQuery, string>
{
    private readonly UserManager<AppUser> _userManager;

    public GetUserWeightQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<string> Handle(GetUserWeightQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user.Weight <= 0 || request.QueryExecutingType.Equals(QueryExecutingTypes.QueryAsResponseForCommand))
        {
            return "Please, set your weight before getting it."; 
        }

        return $"Your weight is {user.Weight}";
    }
}
