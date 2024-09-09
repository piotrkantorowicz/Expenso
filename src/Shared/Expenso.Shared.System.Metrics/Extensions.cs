using Microsoft.Extensions.DependencyInjection;

using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Expenso.Shared.System.Metrics;

public static class Extensions
{
    public static IServiceCollection AddOtlpMetrics(this IServiceCollection serviceCollection,
        OtlpSettings otlpSettings)
    {
        serviceCollection
            .AddOpenTelemetry()
            .ConfigureResource(configure: resourceBuilder =>
                AppResourceBuilder(resource: resourceBuilder, otlpSettings: otlpSettings))
            .WithTracing(configure: builder => builder
                .AddAspNetCoreInstrumentation()
                .AddKeycloakAuthServicesInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSource("APITracing")
                .AddConsoleExporter()
                .AddOtlpExporter(configure: options => options.Endpoint = new Uri(uriString: otlpSettings.Endpoint!)))
            .WithMetrics(configure: builder => builder
                .AddRuntimeInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddKeycloakAuthServicesInstrumentation()
                .AddOtlpExporter(configure: options => options.Endpoint = new Uri(uriString: otlpSettings.Endpoint!)));

        return serviceCollection;
    }

    private static void AppResourceBuilder(ResourceBuilder resource, OtlpSettings otlpSettings)
    {
        resource.AddTelemetrySdk().AddService(serviceName: otlpSettings.ServiceName!);
    }
}