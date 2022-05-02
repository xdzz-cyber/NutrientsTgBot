using Application.Interfaces;
using MediatR;

namespace Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest, ICommand
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public DeleteUserCommand(string username = "", long chatId = 0) => (Username, ChatId) = (username, chatId);
}
