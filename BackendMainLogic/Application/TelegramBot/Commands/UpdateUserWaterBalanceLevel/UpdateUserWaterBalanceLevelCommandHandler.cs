using Application.Common.Constants;
using Application.Interfaces;
using Application.TelegramBot.Queries.GetUserWaterBalanceLevel;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Commands.UpdateUserWaterBalanceLevel;

public class UpdateUserWaterBalanceLevelCommandHandler : IRequestHandler<UpdateUserWaterBalanceLevelCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;

    public UpdateUserWaterBalanceLevelCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager, IMediator mediator)
    {
        _ctx = ctx;
        _userManager = userManager;
        _mediator = mediator;
    }
    
    public async Task<string> Handle(UpdateUserWaterBalanceLevelCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.AmountOfWater.All(char.IsDigit) || double.Parse(request.AmountOfWater) <= 0)
            {
                return "Please, enter correct value";
                // return await _mediator.Send(new GetUserWaterBalanceLevelQuery(username: request.Username, 
                //     chatId: request.ChatId, QueryExecutingTypes.QueryAsResponseForCommand), cancellationToken);
            }
            
            var user = await _userManager.FindByNameAsync(request.Username);
            
            if (user is null)
            {
                return "Please, authorize to be able to make actions.";
            }
            
            var currentWaterLevelBalance = _ctx.WaterLevelOfUsers.FirstOrDefault(x => x.AppUserId == user.Id);

            if (currentWaterLevelBalance is null)
            {
                _ctx.WaterLevelOfUsers.Add(new WaterLevelOfUser()
                {
                    Amount = double.Parse(request.AmountOfWater),
                    AppUserId = user.Id,
                    Id = Guid.NewGuid(),
                    ExpiryDateTime = DateTime.Now.AddDays(1)
                });
                await _ctx.SaveChangesAsync(cancellationToken);
                
                return "New value has been successfully saved";
            }

            currentWaterLevelBalance.Amount += double.Parse(request.AmountOfWater);
            
            _ctx.WaterLevelOfUsers.Update(currentWaterLevelBalance);
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return "An error occured during saving new value. Please, try again";
        }

        return "Existing value has been successfully updated";
    }
}
