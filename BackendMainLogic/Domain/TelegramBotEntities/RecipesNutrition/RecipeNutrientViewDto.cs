using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities.RecipesNutrition;

public class RecipeNutrientViewDto
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("amount")] public float Amount { get; set; }
    
    [JsonPropertyName("unit")] public string Unit { get; set; } = null!;
}
