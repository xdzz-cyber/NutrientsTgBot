using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetApprovedAmountOfNutrients;

public class GetApprovedAmountOfNutrientsQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }
    
    public GetApprovedAmountOfNutrientsQuery(string username, long chatId)
        => (Username, ChatId) = (username, chatId);
}
