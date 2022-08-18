namespace Domain.TelegramBotEntities;

public class NutrientUser
{
    public int NutrientId { get; set; }
    public Nutrient Nutrient { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
    public AppUser AppUser { get; set; } = null!;
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
}
