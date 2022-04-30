using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetUserDetails;
using Application.Users.Queries.GetUserList;
using Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TelegramBotUI.Controllers;

/// <summary>
/// 
/// </summary>
[Produces("application/json")]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UserController : BaseController
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDetailsVm>> GetOne(Guid id)
    {
        var query = new GetUserDetailsQuery {Id = id};
        var result = await Mediator!.Send(query);

        return Ok(result);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserListVm>> GetAll()
    {
        var query = new GetUserListQuery();
        var result = await Mediator!.Send(query);

        return Ok(result);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="updateUserDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserListVm>> Update([FromBody] UpdateUserDto updateUserDto)
    {
        var query = new UpdateUserCommand()
        {
            Email = updateUserDto.Email,
            Id = updateUserDto.Id,
            Password = updateUserDto.Password,
            Phone = updateUserDto.Phone,
            Username = updateUserDto.Username
        };
        var result = await Mediator!.Send(query);

        return Ok(result);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserListVm>> Delete(string id)
    {
        var query = new DeleteUserCommand()
        {
            Id = Guid.Parse(id)
        };
        var result = await Mediator!.Send(query);

        return Ok(result);
    }
}
