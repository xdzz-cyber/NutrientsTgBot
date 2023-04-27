using Application.Common.Constants;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetUserWaterBalanceLevel;

public class GetUserWaterBalanceLevelQueryHandler : IRequestHandler<GetUserWaterBalanceLevelQuery, string>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITelegramBotDbContext _ctx;

    public GetUserWaterBalanceLevelQueryHandler(UserManager<AppUser> userManager, ITelegramBotDbContext ctx)
    {
        _userManager = userManager;
        _ctx = ctx;
    }
    
    public async Task<string> Handle(GetUserWaterBalanceLevelQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        var currentWaterLevelOfUser = await _ctx.WaterLevelOfUsers
            .FirstOrDefaultAsync(x => x.AppUserId == user.Id, cancellationToken);

        if (currentWaterLevelOfUser is null)
        {
            return "No data found.";
        }

        var waterLevelLowerMargin = user.Weight * 30;
        
        var waterLevelUpperMargin = user.Weight * 40;

        var response = "";
        
        if(currentWaterLevelOfUser.Amount >= waterLevelLowerMargin && currentWaterLevelOfUser.Amount <= waterLevelUpperMargin)
        {
            response = $"You have consumed enough water for the current day ({currentWaterLevelOfUser.Amount} ml) till {currentWaterLevelOfUser.ExpiryDateTime}";
        }
        else if (currentWaterLevelOfUser.Amount > waterLevelLowerMargin)
        {
            response = $"You have consumed too much water, please consider taking a break till {currentWaterLevelOfUser.ExpiryDateTime}";
        }
        else
        {
            response = $"You've already consumed {currentWaterLevelOfUser.Amount} ml and yet have to consume {waterLevelLowerMargin} milliliters more till {currentWaterLevelOfUser.ExpiryDateTime}";
        }

        return response;
    }
}
