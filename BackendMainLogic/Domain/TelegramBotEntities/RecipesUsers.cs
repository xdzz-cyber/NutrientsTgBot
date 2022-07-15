namespace Domain.TelegramBotEntities;

public class RecipesUsers
{
    public Recipe Recipe { get; set; }
    
    public int RecipeId { get; set; }

    public AppUser AppUser { get; set; }
    
    public string AppUserId { get; set; }
}
