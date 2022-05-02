using Application.Interfaces;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<Guid>, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string Phone { get; set; }

    public CreateUserCommand(string username = "", long chatId = 0, string email = "", string password = "", string phone = "")
        => (Username, ChatId, Email, Password, Phone) = (username, chatId, email, password, phone);
}
