using Application.TelegramBot.Commands.UpdateUserNutrientsPlan;
using Domain.TelegramBotEntities.NutrientsPlan;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;

[Authorize(Roles = "User")]
public class NutrientsPlanController : Controller
{
    private readonly IMediator _mediator;

    public NutrientsPlanController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public IActionResult NutrientsPlanForm()
    {
        return View("_NutrientsPlanFormPartial");
    }
    
    [HttpPost]
    public async Task<ViewResult> UpdateNutrientsPlan([FromForm] NutrientsPlanFormDto nutrientsPlanFormViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("_ResponseMessageComponent", string.Join("; ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)));
        }
        
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new UpdateUserNutrientsPlanCommand(username!, new NutrientsPlanFormDto
        {
            CaloriesMin = nutrientsPlanFormViewModel.CaloriesMin,
            CaloriesMax = nutrientsPlanFormViewModel.CaloriesMax,
            CarbohydratesMin = nutrientsPlanFormViewModel.CarbohydratesMin,
            CarbohydratesMax = nutrientsPlanFormViewModel.CarbohydratesMax,
            ProteinMin = nutrientsPlanFormViewModel.ProteinMin,
            ProteinMax = nutrientsPlanFormViewModel.ProteinMax,
            FatMin = nutrientsPlanFormViewModel.FatMin,
            FatMax = nutrientsPlanFormViewModel.FatMax
        }));

        return View("_ResponseMessageComponent", result);
    }
}
