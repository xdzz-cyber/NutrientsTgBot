namespace Domain.TelegramBotEntities;

public class WaterLevelOfUser
{
    public Guid Id { get; set; }

    public List<Dictionary<string,double>> Elements { get; set; }

    public double Weight { get; set; }

    // public long QualitativeAmount { get; set; }
    //
    // public long QuantitativeAmount { get; set; }

    public string AppUserId { get; set; }
    
    public AppUser AppUser { get; set; }
}
