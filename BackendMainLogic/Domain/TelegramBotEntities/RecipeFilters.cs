namespace Domain.TelegramBotEntities;

public class RecipeFilters
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<RecipeFiltersUsers> RecipeFiltersUsers { get; set; }
}
