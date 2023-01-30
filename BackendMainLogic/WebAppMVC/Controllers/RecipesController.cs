using Application.TelegramBot.Commands.AddAllLikedRecipesAsMeal;
using Application.TelegramBot.Commands.AddRecipeAsPartOfMeal;
using Application.TelegramBot.Commands.AddRecipeToUser;
using Application.TelegramBot.Commands.ClearLikedRecipesList;
using Application.TelegramBot.Commands.ClearRecipesAsPartOfMeal;
using Application.TelegramBot.Commands.RemoveRecipeFromLikedList;
using Application.TelegramBot.Commands.RemoveRecipeFromTheMeal;
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

        var result = recipes is null || recipes.Count == 0 
            ? await _mediator.Send(new GetUserRecipeListQuery(username!)) : recipes;
        
        return View("RecipesCarousel", new RecipesCarouselViewModel
        {
            Recipes = result
        });
    }

    [HttpGet]
    public async Task<IActionResult> AddRecipeToUser([FromQuery] string recipeId)
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new AddRecipeToUserCommand(username!, recipeId));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpGet]
    public async Task<IActionResult> RecipesByNutrients()
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new GetRecipesByNutrientsQuery(username!));

        return await ShowRecipes(result);
    }

    [HttpGet]
    public async Task<IActionResult> AddRecipeAsPartOfMeal([FromQuery] string recipeId)
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new AddRecipeAsPartOfMealCommand(username!, recipeId));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpGet]
    public async Task<IActionResult> RemoveRecipeFromTheMeal([FromQuery] string recipeId)
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new RemoveRecipeFromTheMealCommand(username!, recipeId));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpGet]
    public async Task<IActionResult> RemoveRecipeFromLikedList([FromQuery] string recipeId)
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new RemoveRecipeFromLikedListCommand(username!, recipeId));

        return View("_ResponseMessageComponent", result);
    }

    [HttpGet]
    public async Task<IActionResult> ClearLikedRecipesList()
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new ClearLikedRecipesListCommand(username!));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpGet]
    public async Task<IActionResult> ClearRecipesAsPartOfMeal()
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new ClearRecipesAsPartOfMealCommand(username!));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpGet]
    public async Task<IActionResult> AddAllLikedRecipesAsMeal()
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new AddAllLikedRecipesAsMealCommand(username!));

        return View("_ResponseMessageComponent", result);
    }
    
    [HttpPost]
    public async Task<IActionResult> RecipesByIngredients([FromForm] string newValue)
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new GetRecipesByIngredientsQuery(username!, newValue));

        return await ShowRecipes(result);
    }
}
