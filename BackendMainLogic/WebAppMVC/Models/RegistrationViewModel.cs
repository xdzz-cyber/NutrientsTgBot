using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models;

public class RegistrationViewModel
{
    
    [Required]
    [MaxLength(10)]
    public string UserName { get; set; } = null!;

    [Required]
    [Range(1,100)]
    public int Age { get; set; }
    [Required]
    public string Password { get; set; }
}
