using Application.Interfaces;
using MediatR;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string Phone { get; set; }

    
    public double Weight { get; set; }
    
    public UpdateUserCommand(string username = "", long chatId = 0, string email = "", string password = "", 
        string phone = "", double weight = 0)
    => (Username,ChatId, Email, Password, Phone, Weight) = (username,chatId, email, password, phone, weight);
}
