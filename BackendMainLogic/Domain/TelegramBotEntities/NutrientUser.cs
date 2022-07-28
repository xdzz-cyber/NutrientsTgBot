namespace Domain.TelegramBotEntities;

public class NutrientUser
{
    public int NutrientId { get; set; }
    public Nutrient Nutrient { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public int MinValue { get; set; }
    
    public int MaxValue { get; set; }
}
