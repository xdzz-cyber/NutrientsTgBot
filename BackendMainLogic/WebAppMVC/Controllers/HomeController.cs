using Application.TelegramBot.Queries.GetApprovedAmountOfNutrients;
using Application.TelegramBot.Queries.GetAvailableRecipeFilters;
using Application.TelegramBot.Queries.GetUserFiltersForRecipes;
using Application.TelegramBot.Queries.GetUserInfo;
using Application.TelegramBot.Queries.GetUserNutrientsPlan;
using Application.TelegramBot.Queries.GetUserSupplementsOutline;
using Application.TelegramBot.Queries.GetUserWaterBalanceLevel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers;


[Authorize(Roles = "User")]
public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ViewResult> Main()
    {
        var username = User.Identity?.Name;
        
        var userInfo = await _mediator.Send(new GetUserInfoQuery(username!));

        var waterBalanceInfo = await _mediator.Send(new GetUserWaterBalanceLevelQuery(username!));

        var nutrientsPlanInfo = await _mediator.Send(new GetUserNutrientsPlanQuery(username!));

        var userRecipesFilters = await _mediator.Send(new GetUserFiltersForRecipesQuery(username!));

        var availableRecipesFilters = await _mediator.Send(new GetAvailableRecipeFiltersQuery(username!));

        var approvedAmountOfNutrients = await _mediator.Send(new GetApprovedAmountOfNutrientsQuery(username!));

        var userSupplementsOutline = await _mediator.Send(new GetUserSupplementsOutlineQuery(username!));
        
        return View(new UserCompleteInfoViewModel
        {
            UserInfo = userInfo,
            WaterBalanceInfo = waterBalanceInfo,
            NutrientsPlanInfo = nutrientsPlanInfo,
            UserRecipesFilters = userRecipesFilters,
            AvailableFiltersForRecipes = availableRecipesFilters,
            ApprovedAmountOfNutrients = approvedAmountOfNutrients,
            UserSupplementsOutline = userSupplementsOutline
        });
    }

    [HttpGet]
    public IActionResult DefaultForm([FromQuery] ParamsForDefaultFormViewModel paramsForDefaultFormViewModel)
    {
        ViewData["ActionName"] = paramsForDefaultFormViewModel.ActionName;
        
        ViewData["ControllerName"] = paramsForDefaultFormViewModel.ControllerName;

        var inputDataFilterMessage = paramsForDefaultFormViewModel.ActionName switch
        {
            "AddRecipeFiltersToUser" => "Please, enter one filter or many with comma as a separator.",
            "UpdateGender" => "Please, enter correct sex (man or women).",
            _ => "Please, enter correct number"
        };

        ViewData["InputDataFilterMessage"] = inputDataFilterMessage;

        return View("_DefaultFormPartial");
    }

    [HttpGet]
    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ShowResult([FromQuery] string result)
    {
        return View("_ResponseMessageComponent", result);
    }
}
