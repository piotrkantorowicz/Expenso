namespace Expenso.BudgetSharing.Core.Models;

internal sealed record BudgetPermission
{
    public Guid BudgetPermissionId { get; set; }
    
    public Guid BudgetId { get; }

    public Guid OwnerId { get; }

    public ICollection<Person> SubOwners { get; }

    public ICollection<Person> Reviewers { get; }
} 