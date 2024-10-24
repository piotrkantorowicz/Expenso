using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.BudgetSharing.Application.Shared.Settings;

public sealed record BudgetSharingSettings : ISettings
{
    public int ExpirationDays { get; init; }
}