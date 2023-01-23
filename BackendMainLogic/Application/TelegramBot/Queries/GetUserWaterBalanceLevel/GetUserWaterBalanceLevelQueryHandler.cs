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
        
        // if (user is null)
        // {
        //     return "Please, authorize to be able to make actions.";
        // }
        
        var currentWaterLevelOfUser = await _ctx.WaterLevelOfUsers
            .FirstOrDefaultAsync(x => x.AppUserId == user.Id, cancellationToken);

        if (currentWaterLevelOfUser?.ExpiryDateTime < DateTime.Now)
        {
            _ctx.WaterLevelOfUsers.Remove(currentWaterLevelOfUser);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        // if (request.QueryExecutingType.Equals(QueryExecutingTypes.QueryAsResponseForCommand) 
        //     || currentWaterLevelOfUser is null)
        // {
        //     return "Please, set amount of consumed water during current day in milliliters before getting it.";
        // }

        var waterLevelMargin =  WaterLevelBalanceConstants.WaterLevelBalanceFormulaConstant * 
            user.Weight - Math.Abs(currentWaterLevelOfUser!.Amount);
        
        return  waterLevelMargin <= 0 ? 
            $"You have consumed enough water for the current day ({currentWaterLevelOfUser.Amount} ml)" : 
            $"You've already consumed {currentWaterLevelOfUser.Amount} and yet have to consume {waterLevelMargin} milliliters more";
    }
}
