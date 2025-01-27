namespace Expenso.TimeManagement.Core.Domain.JobEntries.Model;

internal sealed record JobInstance
{
    public static JobInstance Default => new()
    {
        Id = new Guid(g: "0194a81a-48c6-7999-a548-83dcdd75efa9"),
        Name = "Default",
        RunningDelay = 10
    };

    public Guid Id { get; init; }

    public string? Name { get; init; }

    public int RunningDelay { get; init; }
}