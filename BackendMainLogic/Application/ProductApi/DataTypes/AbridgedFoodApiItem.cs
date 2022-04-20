using System.Text.Json.Serialization;

namespace Application.ProductApi.DataTypes
{
    public class AbridgedFoodApiItem
    {
        [JsonPropertyName("fdcId")]
        public int FdcId { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("dataType")]
        public string DataType { get; set; } = string.Empty;
        [JsonPropertyName("publicationDate")]
        public string PublicationDate { get; set; } = string.Empty;
        [JsonPropertyName("ndbNumber")]
        public string NdbNumber { get; set; } = string.Empty;
        [JsonPropertyName("foodNutrients")]
        public List<AbridgedNutrientApiItem> Nutrients { get; set; } = new();
    }
}
