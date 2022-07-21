namespace Domain.TelegramBotEntities;

public class Meal
{
    public int Id { get; set; }

    public string Calories { get; set; }
    
    public string Fat { get; set; }
    
    public string Carbs { get; set; }
    
    public string Protein { get; set; }

    public AppUser AppUser;

    public string AppUserId { get; set; }
}
