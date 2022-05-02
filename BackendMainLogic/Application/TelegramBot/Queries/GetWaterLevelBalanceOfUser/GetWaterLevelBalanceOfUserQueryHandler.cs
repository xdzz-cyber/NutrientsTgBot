using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetWaterLevelBalanceOfUser;

public class GetWaterLevelBalanceOfUserQueryHandler : IRequestHandler<GetWaterLevelBalanceOfUserQuery,long>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITelegramBotDbContext _ctx;

    public GetWaterLevelBalanceOfUserQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<long> Handle(GetWaterLevelBalanceOfUserQuery request, CancellationToken cancellationToken)
    {
        // var user = await _userManager.FindByNameAsync(request.Username);
        //
        // if (user is null)
        // {
        //     throw new NotFoundException(nameof(AppUser), request.Username);
        // }
        //
        // var userWaterLevel = await _ctx.WaterLevelOfUsers.FirstOrDefaultAsync(x => x.AppUserId == user.Id, cancellationToken);
        //
        // if (userWaterLevel is null)
        // {
        //     var waterLevelOfUser = new WaterLevelOfUser()
        //     {
        //         AppUserId = user.Id,
        //         
        //     }
        // }
        return 0;
    }
}
