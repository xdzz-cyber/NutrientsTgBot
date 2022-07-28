using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetUserNutrientsPlan;

public class GetUserNutrientsPlanQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserNutrientsPlanQuery(string username, long chatId)
        => (Username, ChatId) = (username, chatId);
}
