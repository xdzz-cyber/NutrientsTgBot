using Application.Interfaces;
using MediatR;

namespace Application.TelegramBot.Queries.GetAvailableRecipeFilters;

public class GetAvailableRecipeFiltersQuery : IRequest<string>, IQuery
{
    public string Username { get; set; }

    public long ChatId { get; set; }

    public GetAvailableRecipeFiltersQuery(string username, long chatId = 0)
        => (Username, ChatId) = (username, chatId);
}
