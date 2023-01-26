using Application.Interfaces;
using Domain.TelegramBotEntities.RecipesFilters;
using MediatR;

namespace Application.TelegramBot.Queries.GetUserFiltersForRecipes;

public class GetUserFiltersForRecipesQuery : IRequest<IList<UserRecipesFiltersViewDto>>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserFiltersForRecipesQuery(string username, long chatId = 0) => (Username, ChatId) = (username, chatId);
}
