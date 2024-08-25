namespace Expenso.Shared.Domain.Types.ValueObjects;

public sealed record Blocker(bool IsBlocked, DateTimeOffset? BlockDate)
{
    public static Blocker Block(DateTimeOffset dateTime)
    {
        return new Blocker(IsBlocked: true, BlockDate: dateTime);
    }
}