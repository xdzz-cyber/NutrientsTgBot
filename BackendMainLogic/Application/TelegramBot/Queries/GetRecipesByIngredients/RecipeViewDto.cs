using Application.Common.Mappings;
using AutoMapper;

namespace Domain.TelegramBotEntities;

public class RecipeViewDto : IMapWith<Recipe>
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    
    public string SpoonacularSourceUrl { get; set; } = null!;

    public double PricePerServing { get; set; }

    public bool GlutenFree { get; set; }

    public bool Vegetarian { get; set; }

    public void Mapping(Profile profile) => profile.CreateMap<Recipe, RecipeViewDto>()
        .ForMember(x => x.Id,
            opt => opt.MapFrom(x => x.Id)).ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title))
        .ForMember(x => x.Vegetarian, opt => opt.MapFrom(x => x.Vegetarian))
        .ForMember(x => x.GlutenFree, opt => opt.MapFrom(x => x.GlutenFree))
        .ForMember(x => x.PricePerServing, opt => opt.MapFrom(x => x.PricePerServing))
        .ForMember(x => x.SpoonacularSourceUrl, opt => opt.MapFrom(x => x.SpoonacularSourceUrl));
}
