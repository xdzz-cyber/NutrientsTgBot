using System.Text.Json.Serialization;

namespace Application.ProductApi.DataTypes
{
    public class AbridgedNutrientApiItem
    {
        [JsonPropertyName("number")]
        public string Number { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        [JsonPropertyName("unitName")]
        public string UnitName { get; set; } = string.Empty;
        [JsonPropertyName("derivationCode")]
        public string DerivationCode { get; set; } = string.Empty;
        [JsonPropertyName("derivationDescription")]
        public string DerivationDescription { get; set; } = string.Empty;
    }
}
