using System.Text.Json.Serialization;

namespace Application.TelegramBot.Queries.Dtos.RecipesNutrition;

public class RecipesNutrientsList
{
    [JsonPropertyName("nutrients")] public List<RecipeNutrientViewDto> Nutrients { get; set; } = null!;
}
