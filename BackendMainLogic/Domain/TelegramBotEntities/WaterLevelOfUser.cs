namespace Domain.TelegramBotEntities;

public class WaterLevelOfUser
{
    public Guid Id { get; set; }
    public double Amount { get; set; }
    public string AppUserId { get; set; }
    
    public AppUser AppUser { get; set; }

    public DateTime ExpiryDateTime { get; set; }
}
