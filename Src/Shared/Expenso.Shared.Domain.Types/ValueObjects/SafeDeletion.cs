namespace Expenso.Shared.Domain.Types.ValueObjects;

public sealed record SafeDeletion(bool IsDeleted, DateTimeOffset? RemovalDate)
{
    public static SafeDeletion Delete(DateTimeOffset dateTime)
    {
        return new SafeDeletion(IsDeleted: true, RemovalDate: dateTime);
    }
}