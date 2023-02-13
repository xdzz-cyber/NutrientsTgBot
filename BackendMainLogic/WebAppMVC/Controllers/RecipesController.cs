using System.Text.Json;
using Application.TelegramBot.Commands.AddAllLikedRecipesAsMeal;
using Application.TelegramBot.Commands.AddRecipeAsPartOfMeal;
using Application.TelegramBot.Commands.AddRecipesToUser;
using Application.TelegramBot.Commands.AddRecipeToUser;
using Application.TelegramBot.Commands.ClearLikedRecipesList;
using Application.TelegramBot.Commands.ClearRecipesAsPartOfMeal;
using Application.TelegramBot.Commands.RemoveRecipeFromLikedList;
using Application.TelegramBot.Commands.RemoveRecipeFromTheMeal;
using Application.TelegramBot.Queries.GetMealPlanForUser;
using Application.TelegramBot.Queries.GetRecipesAsPartOfMeal;
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
    public async Task<IActionResult> ShowRecipes(int pageNumber = 1)
    {
        
        var username = User.Identity?.Name;

        if (HttpContext.Session.GetString("CurrentRecipes") is null)
        {
            HttpContext.Session.SetString("CurrentRecipes", JsonSerializer
                .Serialize(await _mediator.Send(new GetUserRecipeListQuery(username!))));
        }

        if (HttpContext.Session.GetString("Nutrients") is null)
        {
            HttpContext.Session.SetString("Nutrients",JsonSerializer.Serialize(new NutrientViewDto()));
        }

        var recipes = JsonSerializer.Deserialize<List<Recipe>>(HttpContext.Session.GetString("CurrentRecipes") ?? throw new InvalidOperationException());

        var nutrients = JsonSerializer.Deserialize<NutrientViewDto>(HttpContext.Session.GetString("Nutrients") ?? throw new InvalidOperationException());

        return View("RecipesCarousel", new RecipesCarouselViewModel
        {
            Recipes = recipes!.Skip((pageNumber - 1) * 3).Take(3).ToList(),
            NutrientViewDto = nutrients,
            MaxRecipesPerPage = 3,
            CurrentPageNumber = pageNumber,
            TotalRecipesCount = recipes!.Count
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

        HttpContext.Session.SetString("CurrentRecipes", JsonSerializer.Serialize(result));

        return await ShowRecipes();
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
    public async Task<IActionResult> RecipesAsPartOfMeal()
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new GetRecipesAsPartOfMealQuery(username!));

        HttpContext.Session.SetString("CurrentRecipes",JsonSerializer.Serialize(result));

        return await ShowRecipes();
    }

    [HttpGet]
    public async Task<IActionResult> MealPlanForUser()
    {
        var username = User.Identity?.Name;

        var result = await _mediator.Send(new GetMealPlanForUserQuery(username!));
        
        HttpContext.Session.SetString("CurrentRecipes",JsonSerializer.Serialize(result.Item1));
        
        HttpContext.Session.SetString("Nutrients",JsonSerializer.Serialize(result.Item2));

        return await ShowRecipes();
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

        HttpContext.Session.SetString("CurrentRecipes", JsonSerializer.Serialize(result));

        return await ShowRecipes();
    }
    
    [HttpPost]
    public async Task<string> AddRecipesToUser([FromBody] string recipeIds)
    {
        var username = User.Identity?.Name;
        
        var recipeIdsList = JsonSerializer.Deserialize<List<string>>(recipeIds);

        return await _mediator.Send(new AddRecipesToUserCommand(username!, recipeIdsList!));
    }
}
