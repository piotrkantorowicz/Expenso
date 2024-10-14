using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace Expenso.Shared.System.Logging.Serilog;

public static class Extensions
{
    public static IHostBuilder AddSerilogLogger(this IHostBuilder builder, string? otlpEndpoint = null,
        string? otlpService = null, bool useOpenTelemetry = true)
    {
        builder.UseSerilog(configureLogger: (hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(configuration: hostingContext.Configuration);

            if (!useOpenTelemetry)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(value: otlpEndpoint) || string.IsNullOrWhiteSpace(value: otlpService))
            {
                return;
            }

            loggerConfiguration.WriteToOpenTelemetry(otlpEndpoint: otlpEndpoint, otlpService: otlpService,
                useOpenTelemetry: useOpenTelemetry);
        });

        return builder;
    }

    public static LoggerConfiguration WriteToOpenTelemetry(this LoggerConfiguration loggerConfiguration,
        string? otlpEndpoint = null, string? otlpService = null, bool useOpenTelemetry = true)
    {
        if (!useOpenTelemetry)
        {
            return loggerConfiguration;
        }

        if (string.IsNullOrWhiteSpace(value: otlpEndpoint) || string.IsNullOrWhiteSpace(value: otlpService))
        {
            return loggerConfiguration;
        }

        loggerConfiguration.WriteTo.OpenTelemetry(configure: options =>
        {
            options.Endpoint = $"{otlpEndpoint}/v1/logs";
            options.Protocol = OtlpProtocol.Grpc;

            options.ResourceAttributes = new Dictionary<string, object>
            {
                [key: "service.name"] = otlpService
            };
        });

        return loggerConfiguration;
    }
}