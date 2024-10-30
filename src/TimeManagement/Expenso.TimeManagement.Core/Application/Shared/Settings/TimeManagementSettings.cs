using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.TimeManagement.Core.Application.Shared.Settings;

public sealed record TimeManagementSettings : ISettings
{
    public AllowedEventType[]? AllowedEvents { get; init; }
}