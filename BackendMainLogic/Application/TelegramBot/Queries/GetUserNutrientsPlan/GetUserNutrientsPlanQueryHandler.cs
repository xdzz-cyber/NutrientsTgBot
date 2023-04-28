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

            nutrientName = nutrientName switch
            {
                "Carbohydrates" => "Carbs",
                _ => nutrientName
            };

            var unitsOfMeasurements = nutrientName switch
            {
                "Calories" => "kcal",
                _ => "g"
            };

            response.AppendLine($"{nutrientName}: max = {userNutrient.MaxValue} {unitsOfMeasurements}, min = {userNutrient.MinValue} {unitsOfMeasurements}");
        }

        return response.ToString();
    }
}
