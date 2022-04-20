namespace Application.Interfaces;

public interface ITelegramBotDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
