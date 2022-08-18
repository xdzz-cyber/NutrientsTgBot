using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities.RecipesNutrition;

public class RecipesNutrientsList
{
    [JsonPropertyName("nutrients")] public List<RecipeNutrientViewDto> Nutrients { get; set; } = null!;
}
