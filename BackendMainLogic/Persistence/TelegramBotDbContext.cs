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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<RecipesUsers>()
            .HasKey(ru => new { ru.RecipeId, ru.AppUserId });  
        builder.Entity<RecipesUsers>()
            .HasOne(ru => ru.Recipe)
            .WithMany(r => r.RecipesUsers)
            .HasForeignKey(ru => ru.RecipeId);  
        builder.Entity<RecipesUsers>()
            .HasOne(ru => ru.AppUser)
            .WithMany(au => au.RecipesUsers)
            .HasForeignKey(ru => ru.AppUserId);
        
        builder.Entity<RecipeFiltersUsers>()
            .HasKey(rfu => new {rfu.AppUserId, rfu.RecipeFiltersId });  
        builder.Entity<RecipeFiltersUsers>()
            .HasOne(rfu => rfu.AppUser)
            .WithMany(user => user.RecipeFiltersUsers)
            .HasForeignKey(rfu => rfu.AppUserId);  
        builder.Entity<RecipeFiltersUsers>()
            .HasOne(rfu => rfu.RecipeFilters)
            .WithMany(rf => rf.RecipeFiltersUsers)
            .HasForeignKey(rfu => rfu.RecipeFiltersId);
    }

    public DbSet<WaterLevelOfUser> WaterLevelOfUsers { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    
    public DbSet<Meal> Meals { get; set; }
    public DbSet<RecipesUsers> RecipesUsers { get; set; }
    public DbSet<RecipeFilters> RecipeFilters { get; set; }
    public DbSet<RecipeFiltersUsers> RecipeFiltersUsers { get; set; }

    // public DbSet<AppUser> AppUsers { get; set; }
}
