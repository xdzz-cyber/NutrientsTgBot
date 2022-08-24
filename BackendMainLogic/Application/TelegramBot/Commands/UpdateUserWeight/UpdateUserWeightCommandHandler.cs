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

        if (appUser is null)
        {
            return "Please, authorize to be able to make actions.";
        }

        if (!request.Weight.All(char.IsDigit) ||  double.Parse(request.Weight) <= 0)
        {
            return await _mediator.Send(new GetUserWeightQuery(username: request.Username, 
                chatId: request.ChatId, QueryExecutingTypes.QueryAsResponseForCommand), cancellationToken);
        }
        
        appUser.Weight = double.Parse(request.Weight);

        var _ = await _userManager.UpdateAsync(appUser);

        return _.Succeeded ? "New weight has been successfully saved!" : "Bad data given, please try again.";
    }
}
