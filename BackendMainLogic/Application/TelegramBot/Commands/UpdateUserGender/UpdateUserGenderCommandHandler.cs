using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Commands.UpdateUserGender;

public class UpdateUserGenderCommandHandler : IRequestHandler<UpdateUserGenderCommand, string>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateUserGenderCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<string> Handle(UpdateUserGenderCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null)
        {
            return "Please, authorize yourself.";
        }
        
        if (request.Sex.ToLower() != "man" && request.Sex.ToLower() != "women")
        {
            return "Please, enter correct sex (man or women).";
        }

        user.Sex = request.Sex;

        await _userManager.UpdateAsync(user);

        return "Sex has been successfully update";
    }
}
