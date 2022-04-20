using System.Reflection;
using AutoMapper;

namespace Application.Common.Mappings;

public class AssemblyMappingProfile : Profile
{
    public AssemblyMappingProfile(Assembly assembly)
    {
        ApplyMappingsFromAssemblies(assembly);
    }

    private void ApplyMappingsFromAssemblies(Assembly assembly)
    {
        var types = assembly.GetExportedTypes().Where(type =>
                type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping");
            _ = methodInfo?.Invoke(instance, new[] { this });
        }
    }
}
