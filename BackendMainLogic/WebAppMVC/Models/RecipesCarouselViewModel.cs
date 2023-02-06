using Domain.TelegramBotEntities;

namespace WebAppMVC.Models;

public class RecipesCarouselViewModel
{
    public List<Recipe> Recipes { get; set; } = null!;
    public NutrientViewDto? NutrientViewDto { get; set; }
    public int MaxRecipesPerPage { get; set; }
    public int CurrentPageNumber { get; set; }

    public int TotalRecipesCount { get; set; }
}
