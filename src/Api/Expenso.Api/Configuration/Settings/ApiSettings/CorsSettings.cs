using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Api.Configuration.Settings.ApiSettings;

internal sealed record CorsSettings : ISettings
{
    public bool? Enabled { get; init; }

    public string[]? AllowedOrigins { get; init; }
}