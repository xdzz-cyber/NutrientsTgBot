using MediatR;

namespace Application.TelegramBot.Queries.NotifyUserToSetNewWeight;

public class NotifyUserToSetNewWeightQueryHandler : IRequestHandler<NotifyUserToSetNewWeightQuery, string>
{
    public async Task<string> Handle(NotifyUserToSetNewWeightQuery request, CancellationToken cancellationToken)
    {
        return "Please, enter your weight.";
    }
}
