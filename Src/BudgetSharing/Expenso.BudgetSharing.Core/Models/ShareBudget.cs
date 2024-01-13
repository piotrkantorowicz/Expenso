namespace Expenso.BudgetSharing.Core.Models;

internal sealed record ShareBudget
{
    public Guid ShareBudgetId { get; }

    public Guid BudgetId { get; }

    public Guid OwnerId { get; }

    public ICollection<Guid> SubOwners { get; }

    public ICollection<Guid> Reviewers { get; }
}