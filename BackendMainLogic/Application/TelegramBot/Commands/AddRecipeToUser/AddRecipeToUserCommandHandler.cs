using MediatR;

namespace Application.TelegramBot.Commands.AddRecipeToUser;

public class AddRecipeToUserCommandHandler : IRequestHandler<AddRecipeToUserCommand, string>
{
    public async Task<string> Handle(AddRecipeToUserCommand request, CancellationToken cancellationToken)
    {
        Console.Write("");

        return "a";
    }
}
