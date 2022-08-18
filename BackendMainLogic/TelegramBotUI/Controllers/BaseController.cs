using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TelegramBotUI.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]

public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    protected IMediator? Mediator => HttpContext.RequestServices.GetService<IMediator>();

    internal Guid UserId => !User.Identity!.IsAuthenticated
        ? Guid.Empty
        : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
}
