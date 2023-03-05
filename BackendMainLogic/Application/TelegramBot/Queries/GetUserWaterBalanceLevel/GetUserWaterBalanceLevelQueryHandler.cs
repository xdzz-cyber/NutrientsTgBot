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

        // if (currentWaterLevelOfUser.ExpiryDateTime < DateTime.Now)
        // {
        //     currentWaterLevelOfUser.Amount = 0;
        //     await _ctx.SaveChangesAsync(cancellationToken);
        // }

        var waterLevelMargin =  WaterLevelBalanceConstants.WaterLevelBalanceFormulaConstant * 
            user.Weight - Math.Abs(currentWaterLevelOfUser.Amount);
        
        return  waterLevelMargin <= 0 ? 
            $"You have consumed enough water for the current day ({currentWaterLevelOfUser.Amount} ml) till {currentWaterLevelOfUser.ExpiryDateTime}" : 
            $"You've already consumed {currentWaterLevelOfUser.Amount} and yet have to consume {waterLevelMargin} milliliters more till {currentWaterLevelOfUser.ExpiryDateTime}";
    }
}
