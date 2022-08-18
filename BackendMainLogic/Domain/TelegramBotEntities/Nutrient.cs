namespace Domain.TelegramBotEntities;

public class Nutrient
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<NutrientUser> NutrientUsers { get; set; } = null!;
}
