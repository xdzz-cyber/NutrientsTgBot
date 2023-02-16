using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities.RecipesNutrition;

public class RecipesNutrition
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("vegetarian")] public bool Vegetarian { get; set; }
    
    [JsonPropertyName("glutenFree")] public bool GlutenFree { get; set; }
    
    [JsonPropertyName("image")] public string Image { get; set; } = null!;
    
    [JsonPropertyName("title")] public string Title { get; set; } = null!;

    [JsonPropertyName("nutrition")]
    public RecipesNutrientsList Nutrition { get; set; } = null!;
}
