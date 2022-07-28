using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities;

public class RecipeByNutrientsDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("Title")] public string Title { get; set; } = null!;
    public bool Vegetarian { get; set; }
    public bool GlutenFree { get; set; }
    
}
