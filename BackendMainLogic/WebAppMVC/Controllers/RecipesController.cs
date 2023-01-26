using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;

public class RecipesController : Controller
{
    [HttpGet]
    public IActionResult ShowRecipes()
    {
        return View("RecipesCarousel");
    }
}
