using Application.TelegramBot.Queries.GetRecipesByIngredients;
using Application.TelegramBot.Queries.GetRecipesByNutrients;
using Application.TelegramBot.Queries.GetUserRecipeList;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers;

public class RecipesController : Controller
{
    private readonly IMediator _mediator;

    public RecipesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> ShowRecipes(List<Recipe>? recipes)
    {
        var username = User.Identity?.Name;

        var result = recipes ?? await _mediator.Send(new GetUserRecipeListQuery(username!));
        
        return View("RecipesCarousel", new RecipesCarouselViewModel
        {
            Recipes = result
        });
    }
    
    [HttpGet]
    public async Task<IActionResult> RecipesByNutrients()
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new GetRecipesByNutrientsQuery(username!));

        return await ShowRecipes(result);
    }

    [HttpPost]
    public async Task<IActionResult> RecipesByIngredients([FromForm] string newValue)
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new GetRecipesByIngredientsQuery(username!, newValue));

        return await ShowRecipes(result);
    }
}
