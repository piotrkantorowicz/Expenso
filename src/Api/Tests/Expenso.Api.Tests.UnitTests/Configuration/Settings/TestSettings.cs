using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings;

internal sealed record TestSettings : ISettings
{
    public bool? IsEnabled { get; init; }

    public string? Name { get; init; }
}