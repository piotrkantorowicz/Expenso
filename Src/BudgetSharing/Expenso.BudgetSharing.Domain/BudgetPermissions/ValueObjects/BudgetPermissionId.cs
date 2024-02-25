using Expenso.BudgetSharing.Domain.Shared.Model.Rules;
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
        DomainModelState.CheckBusinessRules([new EmptyIdentifierCannotBeProcessed(value, typeof(BudgetPermissionId))]);

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