using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;

namespace Application.TelegramBot.Queries.GetRecipesByNutrients;

public class GetRecipesByNutrientsQuery : IRequest<List<Recipe>>, IQuery
{
    public string Username { get; set; }
    public long ChatId { get; set; }
    public GetRecipesByNutrientsQuery(string username, long chatId = 0)
        => (Username, ChatId) = (username,chatId);
}
