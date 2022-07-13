using System.Text.Json.Serialization;

namespace Domain.TelegramBotEntities;

public class RecipesList
{
    [JsonPropertyName("recipes")]
    public IList<Recipe> Recipes { get; set; } = null!;
}
