using Application.Users.Queries.GetUserDetails;
using Application.Users.Queries.GetUserList;
using Microsoft.AspNetCore.Mvc;

namespace TelegramBotUI.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
public class UserController : BaseController
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDetailsVm>> GetOne(Guid id)
    {
        var query = new GetUserDetailsQuery() {Id = id};
        var result = await Mediator.Send(query);

        return Ok(result);
    }
}
