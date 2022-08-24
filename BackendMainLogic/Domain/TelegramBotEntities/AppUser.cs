using Microsoft.AspNetCore.Identity;
namespace Domain.TelegramBotEntities;

public class AppUser : IdentityUser
{
    public long ChatId { get; set; }
    public double Weight { get; set; }

    public double Height { get; set; }

    public int Age { get; set; }

    public string Sex { get; set; }
    public ICollection<RecipesUsers> RecipesUsers { get; set; } = null!;
    public ICollection<RecipeFiltersUsers> RecipeFiltersUsers { get; set; } = null!;
    public ICollection<NutrientUser> NutrientUsers { get; set; } = null!;
}
