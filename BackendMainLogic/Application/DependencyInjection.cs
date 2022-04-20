using System.Reflection;
using Application.Auth;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddSingleton<IGetHashCodeOfString, GetSha256CodeOfPassword>();

        return services;
    }
}
