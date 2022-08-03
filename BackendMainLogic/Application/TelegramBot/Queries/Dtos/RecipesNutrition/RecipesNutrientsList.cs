using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.TelegramBot.Queries.Dtos.RecipesNutrition;

public class RecipesNutrientsList
{
    //[JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("nutrients")] public List<RecipeNutrientViewDto> Nutrients { get; set; } = null!;
}
