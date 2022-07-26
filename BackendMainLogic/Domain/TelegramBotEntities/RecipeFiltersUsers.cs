namespace Domain.TelegramBotEntities;

public class RecipeFiltersUsers
{
    public int RecipeFiltersId { get; set; }
    public RecipeFilters RecipeFilters { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public bool IsTurnedIn { get; set; }
}
