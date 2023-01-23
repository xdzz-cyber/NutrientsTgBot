using Application.TelegramBot.Queries.GetUserInfo;
using Application.TelegramBot.Queries.GetUserNutrientsPlan;
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
        
        return View(new UserCompleteInfoViewModel()
        {
            UserInfo = userInfo,
            WaterBalanceInfo = waterBalanceInfo,
            NutrientsPlanInfo = nutrientsPlanInfo
        });
    }

    [HttpGet]
    public IActionResult DefaultForm([FromQuery] ParamsForDefaultFormViewModel paramsForDefaultFormViewModel)
    {
        ViewData["ActionName"] = paramsForDefaultFormViewModel.ActionName;
        
        ViewData["ControllerName"] = paramsForDefaultFormViewModel.ControllerName;
        
        return View("_DefaultFormPartial");
    }
}
