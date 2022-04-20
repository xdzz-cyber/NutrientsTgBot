using Application.Users.Queries.GetUserDetails;
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
        var query = new GetUserDetailsQuery() {Id = id};
        var result = await Mediator!.Send(query);

        return Ok(result);
    }
}
