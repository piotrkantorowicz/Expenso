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

            if (string.IsNullOrEmpty(value: otlpEndpoint) || string.IsNullOrEmpty(value: otlpService))
            {
                return;
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
        });

        return builder;
    }
}