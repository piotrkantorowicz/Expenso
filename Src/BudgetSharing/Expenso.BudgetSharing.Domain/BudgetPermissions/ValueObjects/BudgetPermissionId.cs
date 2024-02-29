using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.Shared.Domain.Types.Model;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;

public sealed record BudgetPermissionId
{
    private BudgetPermissionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static BudgetPermissionId New(Guid value)
    {
        DomainModelState.CheckBusinessRules([
            (new EmptyIdentifierCannotBeProcessed(value, typeof(BudgetPermissionId)), false)
        ]);

        return new BudgetPermissionId(value);
    }

    public static BudgetPermissionId? Nullable(Guid? value)
    {
        return value is null || value == Guid.Empty ? null : new BudgetPermissionId(value.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}