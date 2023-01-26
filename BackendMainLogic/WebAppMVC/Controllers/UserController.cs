using Application.TelegramBot.Commands.UpdateUserAge;
using Application.TelegramBot.Commands.UpdateUserGender;
using Application.TelegramBot.Commands.UpdateUserTallness;
using Application.TelegramBot.Commands.UpdateUserWaterBalanceLevel;
using Application.TelegramBot.Commands.UpdateUserWeight;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;

[Authorize(Roles = "User")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateWeight([FromForm] string newValue)
    {
        var username = User.Identity?.Name;
        
        var result = await _mediator.Send(new UpdateAppUserWeightCommand(username!,
            newValue));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateAge([FromForm] string newValue)
    {
        var username = User.Identity?.Name;
        
        var result = await _mediator.Send(new UpdateUserAgeCommand(username!, newValue));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateGender([FromForm] string newValue)
    {
        var username = User.Identity?.Name;
        
        var result = await _mediator.Send(new UpdateUserGenderCommand(username!, newValue));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateTallness([FromForm] string newValue)
    {
        var username = User.Identity?.Name;
        
        var result = await _mediator.Send(new UpdateUserTallnessCommand(username!, newValue));

        return View("_ResponseMessageComponent", result);
    }


    [HttpPost]
    public async Task<IActionResult> UpdateWaterBalance([FromForm] string newValue)
    {
        var username = User.Identity?.Name;
        
        var result = await _mediator.Send(new UpdateUserWaterBalanceLevelCommand(username!, newValue));

        return View("_ResponseMessageComponent", result);
    }
}
