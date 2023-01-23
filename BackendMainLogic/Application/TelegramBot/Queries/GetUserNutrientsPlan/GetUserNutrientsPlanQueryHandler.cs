using System.Text;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetUserNutrientsPlan;

public class GetUserNutrientsPlanQueryHandler : IRequestHandler<GetUserNutrientsPlanQuery, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public GetUserNutrientsPlanQueryHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(GetUserNutrientsPlanQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await  _userManager.FindByNameAsync(request.Username);
        
        // if (userInfo is null)
        // {
        //     return "Please, authorize to be able to make actions.";
        // }

        var nutrients = await _ctx.Nutrients.ToListAsync(cancellationToken);
        
        var userNutrients = await _ctx.NutrientUsers
            .Where(nu => nu.AppUserId == userInfo.Id).ToListAsync(cancellationToken);

        if (!userNutrients.Any())
        {
            return "You have no nutrients plan";
        }

        var response = new StringBuilder();

        foreach (var userNutrient in userNutrients)
        {
            var nutrientName = nutrients.FirstOrDefault(n => n.Id == userNutrient.NutrientId)?.Name;
            response.AppendLine($"Nutrient name = {nutrientName},Max value = {userNutrient.MaxValue}, " +
                                $"Min Value = {userNutrient.MinValue};");
        }

        return response.ToString();
    }
}
