using Application.TelegramBot.Commands.AddRecipeFiltersToUser;
using Application.TelegramBot.Commands.TurnOffAllRecipeFiltersOfUser;
using Application.TelegramBot.Commands.TurnOffRecipeFilterOfUser;
using Application.TelegramBot.Commands.TurnOnAllRecipeFiltersOfUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;

[Authorize(Roles = "User")]
public class RecipesFiltersController : Controller
{
    private readonly IMediator _mediator;

    public RecipesFiltersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> TurnOffRecipeFilter([FromQuery] string recipeId)
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new TurnOffRecipeFilterOfUserCommand(username!, recipeId));

        return View("_ResponseMessageComponent", result);
    }

    [HttpGet]
    public async Task<IActionResult> TurnOffAllRecipeFilters()
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new TurnOffAllRecipeFiltersOfUserCommand(username!));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpGet]
    public async Task<IActionResult> TurnOnAllFilters()
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new TurnOnAllRecipeFiltersOfUserCommand(username!));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddRecipeFiltersToUser([FromForm] string newValue)
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new AddRecipeFiltersToUserCommand(username!, newValue));

        return View("_ResponseMessageComponent", result);
    }
}
