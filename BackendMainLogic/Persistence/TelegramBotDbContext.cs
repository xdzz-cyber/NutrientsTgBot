using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class TelegramBotDbContext : IdentityDbContext<IdentityUser>, ITelegramBotDbContext
{
    public TelegramBotDbContext(DbContextOptions<TelegramBotDbContext> options) : base(options)
    {
        
    }
}
