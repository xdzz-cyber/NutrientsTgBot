using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Application.TelegramBot.Queries.Dtos.RecipesNutrition;

public class RecipesNutrition
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nutrition")]
    public RecipesNutrientsList Nutrition { get; set; } = null!; 
    //public Dictionary<string, List<JsonDocument>> Nutrition { get; set; } = null!;
}
