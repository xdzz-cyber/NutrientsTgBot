using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models;

public class DefaultFormParamsViewModel
{
    [Required]
    [DataType(DataType.Text)]
    public string NewValue { get; set; } = null!;
}
