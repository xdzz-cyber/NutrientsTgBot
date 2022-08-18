namespace Domain.TelegramBotEntities;

public class Meal
{
    public int Id { get; set; }

    public string Calories { get; set; } = null!;

    public string Fat { get; set; } = null!;

    public string Carbs { get; set; } = null!;

    public string Protein { get; set; } = null!;

    public AppUser AppUser = null!;

    public string AppUserId { get; set; } = null!;
}
