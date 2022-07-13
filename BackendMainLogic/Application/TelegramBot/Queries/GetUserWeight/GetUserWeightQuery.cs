using Application.Common.Constants;
using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetUserWeight;

public class GetUserWeightQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }
    
    public QueryExecutingTypes QueryExecutingType { get; set; }
    

    public GetUserWeightQuery(string username, long chatId, QueryExecutingTypes queryExecutingType = QueryExecutingTypes.Query) 
        => (Username, ChatId, QueryExecutingType) = (username, chatId, queryExecutingType);
}
