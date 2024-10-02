using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Api.Swagger;

public static class Extensions
{
    public static IServiceCollection AddSwaggerDescriptions(this IServiceCollection services)
    {
        services.AddScoped<ISchemaDescriptor, SwaggerSchemaDescriptor>();

        return services;
    }
}