using Domain.TelegramBotEntities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface ITelegramBotDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    public DbSet<WaterLevelOfUser> WaterLevelOfUsers { get; set; }
    
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Meal> Meals { get; set; }
    
    public DbSet<RecipesUsers> RecipesUsers { get; set; }
    public DbSet<RecipeFilters> RecipeFilters { get; set; }
    public DbSet<RecipeFiltersUsers> RecipeFiltersUsers { get; set; }
    public DbSet<Nutrient> Nutrients { get; set; }
    public DbSet<NutrientUser> NutrientUsers { get; set; }
}
