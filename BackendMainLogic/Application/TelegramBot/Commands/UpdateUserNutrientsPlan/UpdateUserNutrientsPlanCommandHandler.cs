using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.UpdateUserNutrientsPlan;

public class UpdateUserNutrientsPlanCommandHandler : IRequestHandler<UpdateUserNutrientsPlanCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public UpdateUserNutrientsPlanCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(UpdateUserNutrientsPlanCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }

        var userNutrients = await _ctx.NutrientUsers
            .Where(nu => nu.AppUserId == userInfo.Id).ToListAsync(cancellationToken);

        if (!_ctx.Nutrients.Any())
        {
            var nutrientsToBeAdded = new List<Nutrient>();

            foreach (var nutrientConstantName in TelegramBotNutrients.Nutrients)
            {
                nutrientsToBeAdded.Add(new Nutrient
                {
                    Name = nutrientConstantName
                });
            }

            await _ctx.Nutrients.AddRangeAsync(nutrientsToBeAdded, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        
        var nutrients = await _ctx.Nutrients.ToListAsync(cancellationToken);

        if (!StateManagement.TempData.ContainsKey("NutrientToBeUpdatedIndex"))
        {
            StateManagement.TempData.Add("NutrientToBeUpdatedIndex", "0");
        }
        
        var nutrientToBeUpdatedIndex = StateManagement.TempData["NutrientToBeUpdatedIndex"].All(char.IsDigit)
             ? int.Parse(StateManagement.TempData["NutrientToBeUpdatedIndex"]) : 0;

        var userNutrientsToBeAdded = new List<NutrientUser>();

        if (Math.Abs(userNutrients.Count - userNutrientsToBeAdded.Count) != nutrients.Count)
        {
            while (Math.Abs(userNutrients.Count - userNutrientsToBeAdded.Count) != nutrients.Count)
            {
                userNutrientsToBeAdded.Add(new NutrientUser
                {
                    AppUserId = userInfo.Id,
                    MaxValue = 0,
                    MinValue = 0,
                    NutrientId = nutrients
                        .FirstOrDefault(n => userNutrientsToBeAdded
                            .All(userNutrientToBeAdded => userNutrientToBeAdded.NutrientId != n.Id))!.Id
                });
            }
            
            await _ctx.NutrientUsers.AddRangeAsync(userNutrientsToBeAdded, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        if (request.Nutrients.Split(',').Length > 1 && nutrientToBeUpdatedIndex != nutrients.Count)
        {
            var userNutrientToBeUpdated = userNutrients
                .First(userNutrient => userNutrient.NutrientId == nutrients[nutrientToBeUpdatedIndex].Id);

            var newNutrientValues = request.Nutrients.Split(',');

            userNutrientToBeUpdated.MinValue = Convert.ToInt32(newNutrientValues[0]);
            userNutrientToBeUpdated.MaxValue = Convert.ToInt32(newNutrientValues[1]);

            await _ctx.SaveChangesAsync(cancellationToken);

            nutrientToBeUpdatedIndex += 1;
            StateManagement.TempData["NutrientToBeUpdatedIndex"] = nutrientToBeUpdatedIndex.ToString() ;
        }
        
        if (nutrientToBeUpdatedIndex != nutrients.Count)
        {
            return $"Please, enter value for min and max {nutrients.Skip(nutrientToBeUpdatedIndex).Take(1).ToList().First().Name} with comma as separator."; //nutrients[nutrientToBeUpdatedIndex].Name
        }

        StateManagement.TempData["NutrientToBeUpdatedIndex"] = "0";

        return "All value have been saved successfully.";
    }
}
