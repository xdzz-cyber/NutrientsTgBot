using Application.Common.Exceptions;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Commands.UpdateAppUserWeight;

public class UpdateAppUserWeightCommandHandler : IRequestHandler<UpdateAppUserWeightCommand, string>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateAppUserWeightCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(UpdateAppUserWeightCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByNameAsync(request.Username);

        if (appUser is null)
        {
            throw new NotFoundException(nameof(AppUser), request.ChatId);
        }

        appUser.Weight = request.Weight;

        var _ = await _userManager.UpdateAsync(appUser);

        return _.Succeeded ? "New weight has been successfully saved!" : "Bad data given, please try again.";
    }
}
