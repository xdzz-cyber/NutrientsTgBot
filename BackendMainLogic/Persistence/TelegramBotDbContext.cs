using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityTypeConfigurations;

namespace Persistence;

public class TelegramBotDbContext : DbContext, ITelegramBotDbContext
{
    public TelegramBotDbContext(DbContextOptions<TelegramBotDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
