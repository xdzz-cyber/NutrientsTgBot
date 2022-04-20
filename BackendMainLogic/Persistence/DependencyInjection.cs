using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TelegramBotDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<ITelegramBotDbContext>(provider => provider.GetService<TelegramBotDbContext>());
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedAccount = false;
        }).AddEntityFrameworkStores<TelegramBotDbContext>().AddDefaultTokenProviders();

        return services;
    }
}
