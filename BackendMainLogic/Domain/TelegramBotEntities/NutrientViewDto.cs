using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities;

public class NutrientViewDto
{
    [JsonPropertyName("calories")] public float Calories { get; set; }

    [JsonPropertyName("protein")] public float Protein { get; set; }

    [JsonPropertyName("fat")] public float Fat { get; set; }

    [JsonPropertyName("carbohydrates")] public float Carbohydrates { get; set; }
}
