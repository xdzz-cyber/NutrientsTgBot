using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers;


[Authorize(Roles = "User")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Main()
    {
        return View();
    }

    [HttpGet]
    public IActionResult DefaultForm([FromQuery] ParamsForDefaultFormViewModel paramsForDefaultFormViewModel)
    {
        ViewData["ActionName"] = paramsForDefaultFormViewModel.ActionName;
        
        ViewData["ControllerName"] = paramsForDefaultFormViewModel.ControllerName;
        
        return View("_DefaultFormPartial");
    }
}
