using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface ITelegramBotDbContext
{
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
