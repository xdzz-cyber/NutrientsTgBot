using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Queries.GetApprovedBmiValue;

public class GetApprovedBmiValueQueryHandler : IRequestHandler<GetApprovedBmiValueQuery, string>
{
    private readonly UserManager<AppUser> _userManager;

    public GetApprovedBmiValueQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<string> Handle(GetApprovedBmiValueQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        var bmiValue = user.Weight / (Math.Pow(user.Height / 100, 2)); // To convert cm into m we had to divide height / 100

        var response = bmiValue switch
        {
            <= 18.5 => "underweight which is a very unhealthy condition, so you need to consult a doctor.",
            > 18.5 and <= 24.9 => "normal which is a healthy condition.",
            >= 25 and <= 29.9 => "overweight which is a unhealthy condition, so you need to lose weight.",
            >= 30 => "obese which is a very unhealthy condition, so you need to lose weight.",
            _ => "very unhealthy condition which is a very unhealthy condition, so you need to consult a doctor."
        };

        return response;
    }
}
