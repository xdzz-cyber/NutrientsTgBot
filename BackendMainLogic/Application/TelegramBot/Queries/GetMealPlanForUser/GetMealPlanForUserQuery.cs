using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetMealPlanForUser;

public class GetMealPlanForUserQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetMealPlanForUserQuery(string username, long chatId)
        => (Username, ChatId) = (username, chatId);
}
