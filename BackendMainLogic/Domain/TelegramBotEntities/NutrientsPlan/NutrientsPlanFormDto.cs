using System.ComponentModel.DataAnnotations;

namespace Domain.TelegramBotEntities.NutrientsPlan;

public class NutrientsPlanFormDto
{
    [Required]
    [DataType(DataType.Text)]
    public string FatMin { get; set; } = null!;
    [Required]
    [DataType(DataType.Text)]
    public string FatMax { get; set; } = null!;
    [Required]
    [DataType(DataType.Text)]
    public string CarbohydratesMin { get; set; } = null!;
    [Required]
    [DataType(DataType.Text)]
    public string CarbohydratesMax { get; set; } = null!;
    [Required]
    [DataType(DataType.Text)]
    public string ProteinMin { get; set; } = null!;
    [Required]
    [DataType(DataType.Text)]
    public string ProteinMax { get; set; } = null!;
    [Required]
    [DataType(DataType.Text)]
    public string CaloriesMin { get; set; } = null!;
    [Required]
    [DataType(DataType.Text)]
    public string CaloriesMax { get; set; } = null!;
}
