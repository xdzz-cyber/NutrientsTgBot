namespace Domain.TelegramBotEntities;

public class UserNutrientPlanDto
{
    public string Name { get; set; } = null!;

    public int MinValue { get; set; }
    
    public int MaxValue { get; set; }
}
