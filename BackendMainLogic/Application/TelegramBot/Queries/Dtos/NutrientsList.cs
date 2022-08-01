using System.Text.Json.Serialization;
using Domain.TelegramBotEntities;

namespace Application.TelegramBot.Queries.Dtos;

public class NutrientsList
{
    [JsonPropertyName("nutrients")]
    public NutrientViewDto Nutrients { get; set; } = null!;
}
