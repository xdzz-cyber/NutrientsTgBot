using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetUserNutrientsPlanReport;

public class GetUserNutrientsPlanReportQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserNutrientsPlanReportQuery(string username, long chatId)
        => (Username, ChatId) = (username, chatId);
}
