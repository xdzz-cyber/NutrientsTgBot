using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;

namespace Application.TelegramBot.Queries.GetMealPlanForUser;

public class GetMealPlanForUserQuery : IRequest<Tuple<List<Recipe>, NutrientViewDto>>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetMealPlanForUserQuery(string username, long chatId = 0)
        => (Username, ChatId) = (username, chatId);
}
