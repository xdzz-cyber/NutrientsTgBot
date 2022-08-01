using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities;

public class MealsList
{
    [JsonPropertyName("meals")]
    public IList<Recipe> Meals { get; set; } = null!;
}
