using Application.Common.Constants;
using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetRecipesByIngredients;

public class GetRecipesByIngredientsQuery : IRequest<string>,IQuery
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public QueryExecutingTypes QueryExecutingType { get; set; }
    public string Ingredients { get; set; }
    
    public GetRecipesByIngredientsQuery(string username, long chatId, string ingredients, QueryExecutingTypes queryExecutingType = QueryExecutingTypes.Query)
        => (Username, ChatId, Ingredients ,QueryExecutingType) = (username, chatId, ingredients, queryExecutingType);
}
