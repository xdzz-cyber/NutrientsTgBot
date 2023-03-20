using Application.TelegramBot.Queries.GetApprovedBmiValue;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Queries.GetUserInfo;

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, string>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;

    public GetUserInfoQueryHandler(UserManager<AppUser> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }
    
    public async Task<string> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user.Weight <= 0 || user.Height <= 0 || user.Age <= 0 || (user.Sex.ToLower() != "man" && user.Sex.ToLower() != "women"))
        {
            return "Please, set all of the values (height, age, weight and sex).";
        }

        var bmiValueResponse = await _mediator
            .Send(new GetApprovedBmiValueQuery(request.Username), cancellationToken);

        return $"Your tallness is {user.Height}cm, age is {user.Age}," +
               $" weight is {user.Weight}kg and sex is {user.Sex.ToLower()}, and BMI value is {bmiValueResponse}";
    }
}
