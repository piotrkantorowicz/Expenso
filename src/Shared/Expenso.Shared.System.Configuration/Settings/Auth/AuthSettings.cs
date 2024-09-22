namespace Expenso.Shared.System.Configuration.Settings.Auth;

public sealed record AuthSettings : ISettings
{
    public AuthServer AuthServer { get; init; }
}