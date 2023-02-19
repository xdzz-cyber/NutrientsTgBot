using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Commands.UpdateUserAge;

public class UpdateUserAgeCommandHandler : IRequestHandler<UpdateUserAgeCommand, string>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateUserAgeCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<string> Handle(UpdateUserAgeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        
        if (user is null)
        {
            return "Please, authorize yourself.";
        }

        if (!request.Age.All(char.IsDigit) || int.Parse(request.Age) <= 0)
        {
            return "Please, enter correct number.";
        }
        
        user.Age = Convert.ToInt32(request.Age);
        
        await _userManager.UpdateAsync(user);

        return "Age has been successfully updated.";
    }
}
