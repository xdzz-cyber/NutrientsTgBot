using Application.Interfaces;
using Domain.TelegramBotEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public class TelegramBotDbContext : IdentityDbContext<AppUser>, ITelegramBotDbContext
{
    public TelegramBotDbContext()
    {
        
    }
    public TelegramBotDbContext(DbContextOptions<TelegramBotDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!);
    }
    
    public DbSet<WaterLevelOfUser> WaterLevelOfUsers { get; set; }
    // public DbSet<AppUser> AppUsers { get; set; }
}
