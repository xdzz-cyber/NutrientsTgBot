using Application.Users.Commands.CreateUser;
using Application.Users.Queries.FindUser;
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
        var newUserId = await _mediator.Send(new CreateUserCommand()
        {
            Username = registrationViewModel.UserName,
            Age = registrationViewModel.Age,
            Password = registrationViewModel.Password
        });
        
        if (!string.IsNullOrEmpty(newUserId.ToString()))
        {
            await _signInManager.SignInAsync(await _userManager.FindByIdAsync(newUserId.ToString()), 
                false);
            
            return RedirectToAction("Main", "Home");
        }

        return View(registrationViewModel);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel)
    {
        var user = await _mediator.Send(new FindUserQuery(loginViewModel.UserName));

        if (user is not null && _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginViewModel.Password) 
            == PasswordVerificationResult.Success)
        {
            await _signInManager.SignInAsync(await _userManager.FindByIdAsync(user.Id),false);

            return RedirectToAction("Main", "Home");
        }

        return View(loginViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Login");
    }
}
