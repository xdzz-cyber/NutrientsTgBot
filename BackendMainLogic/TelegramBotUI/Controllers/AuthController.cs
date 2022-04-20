using Application.Common.Constants;
using Application.Users.Commands.CreateUser;
using Domain.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TelegramBotUI.Controllers;

/// <summary>
/// 
/// </summary>
public class AuthController : BaseController
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="loginUserDto"></param>
    /// <param name="signInManager"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto, [FromServices] SignInManager<IdentityUser> signInManager)
    {
        var result = await signInManager.PasswordSignInAsync(loginUserDto.Username, loginUserDto.Password, false, false);
        return  result.Succeeded ? Ok() : NotFound();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="registerUserDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var createUserCommand = new CreateUserCommand()
        {
            Email = registerUserDto.Email,
            Password = registerUserDto.Password,
            Phone = registerUserDto.Phone,
            Usesrname = registerUserDto.Username
        };

        var result = await Mediator!.Send(createUserCommand);

        return Ok(result);
    }
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="signInManager"></param>
    /// <returns>Http.StatusCode 200</returns>
    /// <returns>Http.StatusCoe 400</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout( [FromServices] SignInManager<IdentityUser> signInManager)
    {
        try
        {
            await signInManager.SignOutAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
        return Ok();
    }
}
