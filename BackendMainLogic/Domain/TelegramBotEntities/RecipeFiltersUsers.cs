namespace Domain.TelegramBotEntities;

public class RecipeFiltersUsers
{
    public int RecipeFiltersId { get; set; }
    public RecipeFilters RecipeFilters { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
    public AppUser AppUser { get; set; } = null!;
    public bool IsTurnedIn { get; set; }
}
