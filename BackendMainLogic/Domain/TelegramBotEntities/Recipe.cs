using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities;

public class Recipe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("cookingMinutes")] public int CookingMinutes { get; set; } = 0;
    
    [JsonPropertyName("sourceName")]
    public string SourceName { get; set; } = "";
    
    [JsonPropertyName("spoonacularSourceUrl")]
    public string SpoonacularSourceUrl { get; set; } = "";

    [JsonPropertyName("sourceUrl")] public string SourceUrl { get; set; } = "";
    [JsonPropertyName("aggregateLikes")] public int AggregateLikes { get; set; } = 0;

    [JsonPropertyName("healthScore")] public int HealthScore { get; set; } = 0;

    [JsonPropertyName("pricePerServing")] public double PricePerServing { get; set; } = 0;

    [JsonPropertyName("glutenFree")] public bool GlutenFree { get; set; } = false;

    [JsonPropertyName("vegetarian")] public bool Vegetarian { get; set; } = false;
    
    [JsonPropertyName("image")] public string Image { get; set; } = null!;
    public ICollection<RecipesUsers> RecipesUsers { get; set; } = new List<RecipesUsers>();
}
