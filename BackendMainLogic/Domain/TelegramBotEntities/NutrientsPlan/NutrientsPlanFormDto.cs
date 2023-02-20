using System.ComponentModel.DataAnnotations;
using FoolProof.Core;

namespace Domain.TelegramBotEntities.NutrientsPlan;

public class NutrientsPlanFormDto
{
    [Required]
    [Range(1,7600)]
    [LessThan("FatMax", ErrorMessage = "Fat min value must be less than max")]
    public int FatMin { get; set; }
    [Required]
    [Range(1,7600)]
    public int FatMax { get; set; } 
    [Required]
    [Range(1,115)]
    [LessThan("CarbohydratesMax", ErrorMessage = "Carbohydrates min value must be less than max")]
    public int CarbohydratesMin { get; set; }
    [Required]
    [Range(1,115)]
    public int CarbohydratesMax { get; set; }
    [Required]
    [Range(1,23)]
    [LessThan("ProteinMax", ErrorMessage = "Protein min value must be less than max")]
    public int ProteinMin { get; set; } 
    [Required]
    [Range(1,23)]
    public int ProteinMax { get; set; }
    [Required]
    [Range(1,60000)]
    [LessThan("CaloriesMax", ErrorMessage = "Calories min value must be less than max")]
    public int CaloriesMin { get; set; }
    [Required]
    [Range(1,60000)]
    public int CaloriesMax { get; set; }
}
