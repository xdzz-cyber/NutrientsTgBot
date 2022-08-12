using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetUserNutrientsCompletePlanReport;

public class GetUserNutrientsCompletePlanReportQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserNutrientsCompletePlanReportQuery(string username, long chatId)
        => (Username, ChatId) = (username, chatId);
}
