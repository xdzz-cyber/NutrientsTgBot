using Application.Users.Commands.CreateUser;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers;

public class AuthController : Controller
{
    private readonly IMediator _mediator;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AuthController(IMediator mediator, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Registration([FromForm] RegistrationViewModel registrationViewModel)
    {
        var newUserId = await _mediator.Send(new CreateUserCommand(username: registrationViewModel.UserName,
            age: registrationViewModel.Age, password: registrationViewModel.Password));
        
        //Later: add [Authorize to controller to which we'll redirect]
        if (!string.IsNullOrEmpty(newUserId.ToString()))
        {
            await _signInManager.SignInAsync(await _userManager.FindByIdAsync(newUserId.ToString()), 
                false);
        }

        return RedirectToAction("");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
}
