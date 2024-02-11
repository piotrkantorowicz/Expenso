using Expenso.Shared.Domain.Types.Events;

namespace Expenso.BudgetSharing.Domain.Shared.Model.Base;

internal sealed class DomainEventsSource
{
    private readonly Queue<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents
    {
        get
        {
            List<IDomainEvent> result = _domainEvents.ToList();
            _domainEvents.Clear();

            return result;
        }
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Enqueue(domainEvent);
    }
}