using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;

public sealed class PermissionId
{
    private readonly DomainModelState _domainModelState = new();

    private PermissionId()
    {
        Value = Guid.Empty;
    }

    private PermissionId(Guid value)
    {
        _domainModelState.CheckBusinessRules([new EmptyIdentifierCannotBeProcessed(value, GetType())]);
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(PermissionId id)
    {
        return id.Value;
    }

    public static implicit operator PermissionId(Guid id)
    {
        return new PermissionId(id);
    }

    internal static PermissionId CreateDefault()
    {
        return new PermissionId();
    }

    public static PermissionId Create(Guid value)
    {
        return new PermissionId(value);
    }
}