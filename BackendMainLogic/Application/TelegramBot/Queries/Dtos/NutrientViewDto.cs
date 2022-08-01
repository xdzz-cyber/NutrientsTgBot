using System.Text.Json.Serialization;
using Application.Common.Mappings;
using AutoMapper;

namespace Application.TelegramBot.Queries.Dtos;

public class NutrientViewDto //: IMapWith<>
{
    [JsonPropertyName("calories")] public float Calories { get; set; }
    
    [JsonPropertyName("protein")] public float Protein { get; set; }

    [JsonPropertyName("fat")] public float Fat { get; set; }

    [JsonPropertyName("carbohydrates")] public float Carbohydrates { get; set; }
    
    //public void Mapping(Profile profile) => profile.CreateMap<>()
}
