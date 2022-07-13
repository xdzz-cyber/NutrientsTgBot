using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities;

public class Recipe
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
    
    [JsonPropertyName("cookingMinutes")]
    public int CookingMinutes { get; set; }
    
    [JsonPropertyName("sourceName")]
    public string SourceName { get; set; } = null!;
    
    [JsonPropertyName("spoonacularSourceUrl")]
    public string SpoonacularSourceUrl { get; set; } = null!;
    
    [JsonPropertyName("aggregateLikes")]
    public int AggregateLikes { get; set; }
    
    [JsonPropertyName("healthScore")]
    public int HealthScore { get; set; }
    
    [JsonPropertyName("pricePerServing")]
    public double PricePerServing { get; set; }
    
    [JsonPropertyName("glutenFree")]
    public bool GlutenFree { get; set; }
    
    [JsonPropertyName("vegetarian")]
    public bool Vegetarian { get; set; }

    public ICollection<AppUser> AppUsers { get; set; }
}
