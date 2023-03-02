using System.Text.Json.Serialization;
using AutoMapper;
using Domain.TelegramBotEntities;

namespace Application.Common.Mappings;

public class RecipeViewDto : IMapWith<Recipe>
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
    
    [JsonPropertyName("spoonacularSourceUrl")]
    public string SpoonacularSourceUrl { get; set; } = null!;
    
    [JsonPropertyName("pricePerServing")]
    public double PricePerServing { get; set; }
    
    [JsonPropertyName("glutenFree")]
    public bool GlutenFree { get; set; }
    
    [JsonPropertyName("vegetarian")]
    public bool Vegetarian { get; set; }
    
    [JsonPropertyName("image")] 
    
    public string Image { get; set; } = null!;

    public void Mapping(Profile profile) => profile.CreateMap<Recipe, RecipeViewDto>()
        .ForMember(x => x.Id,
            opt => opt.MapFrom(x => x.Id)).ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title))
        .ForMember(x => x.Vegetarian, opt => opt.MapFrom(x => x.Vegetarian))
        .ForMember(x => x.GlutenFree, opt => opt.MapFrom(x => x.GlutenFree))
        .ForMember(x => x.PricePerServing, opt => opt.MapFrom(x => x.PricePerServing))
        .ForMember(x => x.SpoonacularSourceUrl, opt => opt.MapFrom(x => x.SpoonacularSourceUrl))
        .ForMember(x => x.Image, opt => opt.MapFrom(x => x.Image));
}
