using MediatR;

namespace Application.TelegramBot.Queries.GetWaterLevelBalanceOfUser;

public class GetWaterLevelBalanceOfUserQuery : IRequest<long>
{
    public string Username { get; set; }

    public long ChatId { get; set; }
    

    public GetWaterLevelBalanceOfUserQuery(string username, long chatId) => (Username, ChatId) = (username, chatId);
}
