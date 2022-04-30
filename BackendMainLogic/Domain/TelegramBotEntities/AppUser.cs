using Microsoft.AspNetCore.Identity;

namespace Domain.TelegramBotEntities;

public class AppUser : IdentityUser
{
    public long ChatId { get; set; }
}
