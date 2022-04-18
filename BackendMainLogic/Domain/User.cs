using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User
{
    public Guid Id { get; set; }

    [Required] [DataType(DataType.Text)] public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }
}
