namespace Expenso.Shared.Domain.Types.ValueObjects;

public sealed record SafeDeletion(bool IsDeleted, DateTimeOffset? RemovalDate)
{
    public static SafeDeletion Delete(DateTimeOffset dateTime)
    {
        return new SafeDeletion(true, dateTime);
    }

    public static SafeDeletion Restore()
    {
        return new SafeDeletion(false, null);
    }
}