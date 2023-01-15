using Application.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers;

public class AuthController : Controller
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Registration([FromBody] RegistrationViewModel registrationViewModel)
    {
        var newUserId = await _mediator.Send(new CreateUserCommand(username: registrationViewModel.UserName,
            age: registrationViewModel.Age, password: registrationViewModel.Password));

        return RedirectToAction("");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
}
