namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed record JobInstance
{
    public static JobInstance Default => new()
    {
        Id = new Guid(g: "d8fc5aed-cc40-4484-864f-945480daa236"),
        Name = "Default",
        RunningDelay = 10
    };

    public Guid Id { get; init; }

    public string? Name { get; init; }

    public int RunningDelay { get; init; }
}