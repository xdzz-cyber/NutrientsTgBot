using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;

namespace Application.TelegramBot.Queries.GetUserRecipeList;

public class GetUserRecipeListQuery : IRequest<List<Recipe>>, IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetUserRecipeListQuery(string username, long chatId = 0) => (Username, ChatId) = (username, chatId);
}
