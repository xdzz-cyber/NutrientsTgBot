using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetRecipesByNutrients;

public class GetRecipesByNutrientsQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public string Nutrients { get; set; }
    public GetRecipesByNutrientsQuery(string username, long chatId, string nutrients)
        => (Username, ChatId, Nutrients) = (username, chatId, nutrients);
}
