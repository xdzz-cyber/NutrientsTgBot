using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Commands.UpdateUserTallness;

public class UpdateUserTallnessCommandHandler : IRequestHandler<UpdateUserTallnessCommand, string>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateUserTallnessCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<string> Handle(UpdateUserTallnessCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (!request.Height.All(char.IsDigit) || int.Parse(request.Height) <= 0)
        {
            return "Please, enter correct number.";
        }

        user.Height = Convert.ToDouble(request.Height);

        await _userManager.UpdateAsync(user);

        return "Tallness has been successfully updated.";
    }
}
