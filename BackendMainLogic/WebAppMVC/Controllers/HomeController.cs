using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;


[Authorize(Roles = "User")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Main()
    {
        return View();
    }
}
