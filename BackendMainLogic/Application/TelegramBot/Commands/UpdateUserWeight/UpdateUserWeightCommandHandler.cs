using Application.Common.Constants;
using Application.TelegramBot.Queries.GetUserWeight;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Commands.UpdateUserWeight;

public class UpdateAppUserWeightCommandHandler : IRequestHandler<UpdateAppUserWeightCommand, string>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;

    public UpdateAppUserWeightCommandHandler(UserManager<AppUser> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<string> Handle(UpdateAppUserWeightCommand request, CancellationToken cancellationToken)
    {

        var appUser = await _userManager.FindByNameAsync(request.Username);

        if (!request.Weight.All(char.IsDigit) ||  double.Parse(request.Weight) <= 0)
        {
            return "Please, enter correct value";
        }
        
        appUser.Weight = double.Parse(request.Weight);

        var identityResult = await _userManager.UpdateAsync(appUser);

        return identityResult.Succeeded ? "New weight has been successfully saved!" : "Bad data given, please try again.";
    }
}
