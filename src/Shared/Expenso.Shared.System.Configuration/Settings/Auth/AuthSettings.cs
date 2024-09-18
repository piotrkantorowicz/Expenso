namespace Expenso.Shared.System.Configuration.Settings.Auth;

public record AuthSettings : ISettings
{
    public AuthServer AuthServer { get; init; }
}