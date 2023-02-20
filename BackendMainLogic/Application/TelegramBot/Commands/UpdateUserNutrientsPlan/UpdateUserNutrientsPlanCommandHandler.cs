﻿using Application.Common.Constants;
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

        var nutrients = await _ctx.Nutrients.ToListAsync(cancellationToken);

        var userNutrients = await _ctx.NutrientUsers
            .Where(nu => nu.AppUserId == userInfo.Id).ToListAsync(cancellationToken);

        if (!_ctx.Nutrients.Any())
        {
            var nutrientsToBeAdded = new List<Nutrient>();

            foreach (var nutrientConstantName in TelegramBotNutrients.Nutrients)
                nutrientsToBeAdded.Add(new Nutrient {Name = nutrientConstantName});

            await _ctx.Nutrients.AddRangeAsync(nutrientsToBeAdded, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        foreach (var nutrient in nutrients)
        {
            var neededValue = nutrient.Name switch
            {
                "Fat" => (min: request.Nutrients.FatMin, max: request.Nutrients.FatMax),
                "Carbohydrates" => (min: request.Nutrients.CarbohydratesMin,
                    max: request.Nutrients.CarbohydratesMax),
                "Protein" => (min: request.Nutrients.ProteinMin, max: request.Nutrients.ProteinMax),
                "Calories" => (min: request.Nutrients.CaloriesMin, max: request.Nutrients.CaloriesMax),
                _ => throw new ArgumentOutOfRangeException(nameof(nutrient.Name), "Bad user nutrient")
            };

            if (userNutrients.All(userNutrient => userNutrient.NutrientId != nutrient.Id))
            {
                await _ctx.NutrientUsers.AddAsync(
                    new NutrientUser
                    {
                        AppUserId = userInfo.Id,
                        MinValue = neededValue.min,
                        MaxValue = neededValue.max,
                        NutrientId = nutrient.Id
                    }, cancellationToken);
            }
            else
            {
                var userNutrientToBeUpdated =
                    userNutrients.First(userNutrient => userNutrient.NutrientId == nutrient.Id);

                userNutrientToBeUpdated.MinValue = neededValue.min;

                userNutrientToBeUpdated.MaxValue = neededValue.max;
            }
        }

        await _ctx.SaveChangesAsync(cancellationToken);

        return "All value have been saved successfully.";
    }
}
