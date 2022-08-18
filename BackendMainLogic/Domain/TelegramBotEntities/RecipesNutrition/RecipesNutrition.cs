using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities.RecipesNutrition;

public class RecipesNutrition
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nutrition")]
    public RecipesNutrientsList Nutrition { get; set; } = null!;
}
