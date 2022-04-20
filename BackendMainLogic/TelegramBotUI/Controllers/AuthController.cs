using Application.Common.Constants;
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
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto, 
        [FromServices] UserManager<IdentityUser> userManager, [FromServices] RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.Roles.Any(x => x.Name.Equals(AuthRoles.User.ToString())))
        {
            var userRole = new IdentityRole(AuthRoles.User.ToString());

            await roleManager.CreateAsync(userRole);
        }
        
        var newUser = new IdentityUser { UserName = registerUserDto.Username, Email = registerUserDto.Email, PhoneNumber = registerUserDto.Phone};
        await userManager.CreateAsync(newUser, registerUserDto.Password);

        var roleName = await roleManager.FindByNameAsync(AuthRoles.User.ToString());
        var finalResult = await userManager.AddToRoleAsync(newUser, roleName.Name);

        return finalResult.Succeeded ? Ok() : BadRequest();
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
