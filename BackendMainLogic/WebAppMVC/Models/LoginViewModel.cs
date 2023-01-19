using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models;

public class LoginViewModel
{
    [Required]
    [MaxLength(10)]
    public string UserName { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
