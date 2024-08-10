using Expenso.Shared.Domain.Types.Events;

namespace Expenso.BudgetSharing.Domain.Shared.Base;

internal sealed class DomainEventsSource
{
    private readonly Queue<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents
    {
        get
        {
            IReadOnlyCollection<IDomainEvent> domainEvents = _domainEvents.ToList().AsReadOnly();
            _domainEvents.Clear();

            return domainEvents;
        }
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Enqueue(item: domainEvent);
    }
}