using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;

namespace Application.TelegramBot.Queries.GetRecipesByIngredients;

public class GetRecipesByIngredientsQuery : IRequest<List<Recipe>>,IQuery
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public QueryExecutingTypes QueryExecutingType { get; set; }
    public string Ingredients { get; set; }
    
    public GetRecipesByIngredientsQuery(string username,string ingredients ,long chatId = 0, QueryExecutingTypes queryExecutingType = QueryExecutingTypes.Query)
        => (Username, Ingredients, ChatId ,QueryExecutingType) = (username, ingredients, chatId, queryExecutingType);
}
