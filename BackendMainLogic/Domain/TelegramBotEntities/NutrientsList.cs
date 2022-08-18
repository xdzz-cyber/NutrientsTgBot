using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities;

public class NutrientsList
{
    [JsonPropertyName("nutrients")]
    public NutrientViewDto Nutrients { get; set; } = null!;
}
