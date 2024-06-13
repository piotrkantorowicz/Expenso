namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed record JobEntryType
{
    public static JobEntryType BudgetSharingRequestExpiration => new()
    {
        Id = Guid.NewGuid(),
        Code = "BS-REQ-EXP",
        Name = "Budget Sharing Requests Expiration",
        RunningDelay = 10
    };

    public Guid Id { get; init; }

    public string? Code { get; init; }

    public string? Name { get; init; }

    public int RunningDelay { get; init; }
}