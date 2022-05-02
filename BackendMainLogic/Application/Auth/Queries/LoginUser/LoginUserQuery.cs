using Application.Interfaces;
using Domain.Auth;
using MediatR;

namespace Application.Auth.Queries.LoginUser;

public class LoginUserQuery :  IRequest<bool>, IQuery
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public LoginUserQuery(string username = "", long chatId = 0) => (Username, ChatId) = (username, chatId);

}
