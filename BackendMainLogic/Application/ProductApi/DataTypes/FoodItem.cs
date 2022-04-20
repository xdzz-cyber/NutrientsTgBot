using System.Text.Json.Serialization;

namespace Application.ProductApi.DataTypes
{
    public class FoodItem
    {
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("amount")] public double Amount { get; set; }
        [JsonPropertyName("unitName")] public string Unit { get; set; } = string.Empty;
        [JsonPropertyName("nutrients")]
        public IEnumerable<FoodNutrient> Nutrients { get; set; } = new List<FoodNutrient>();
    }
}
