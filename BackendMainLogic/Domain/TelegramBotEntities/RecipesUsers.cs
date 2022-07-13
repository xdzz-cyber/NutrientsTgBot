namespace Domain.TelegramBotEntities;

public class RecipesUsers
{
    public int Id { get; set; }

    public Recipe Recipe { get; set; }
    
    public int RecipeId { get; set; }

    public AppUser AppUser { get; set; }
    
    public int AppUserId { get; set; }
}
