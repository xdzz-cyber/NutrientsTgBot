using System.Text.Json.Serialization;

namespace Application.ProductApi.DataTypes
{
    public class FoodNutrient
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        [JsonPropertyName("unitName")]
        public string UnitName { get; set; } = string.Empty;
    }
}
