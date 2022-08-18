namespace Domain.TelegramBotEntities;

public class WaterLevelOfUser
{
    public Guid Id { get; set; }
    public double Amount { get; set; }
    public string AppUserId { get; set; } = null!;

    public AppUser AppUser { get; set; } = null!;
    public DateTime ExpiryDateTime { get; set; }
}
