using System.Text.Json.Serialization;

namespace Application.TelegramBot.Queries.Dtos.RecipesNutrition;

public class RecipesNutrition
{
    [JsonPropertyName("nutrition")] public string Nutrition { get; set; } = null!;
}
