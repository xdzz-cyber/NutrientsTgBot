using Domain.TelegramBotEntities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface ITelegramBotDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    public DbSet<WaterLevelOfUser> WaterLevelOfUsers { get; set; }
    
    public DbSet<Recipe> Recipes { get; set; }
    
    public DbSet<RecipesUsers> RecipesUsers { get; set; }
}
