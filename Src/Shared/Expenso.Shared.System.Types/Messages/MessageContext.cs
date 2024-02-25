using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.ExecutionContext.Models;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.System.Types.Messages;

public sealed record MessageContext : IMessageContext
{
    internal MessageContext(Guid messageId, Guid correlationId, Guid requestedBy, DateTimeOffset timestamp)
    {
        MessageId = messageId;
        CorrelationId = correlationId;
        RequestedBy = requestedBy;
        Timestamp = timestamp;
    }

    internal MessageContext(IExecutionContext? executionContext, IClock clock, Guid? messageId)
    {
        MessageId = messageId ?? Guid.NewGuid();
        CorrelationId = executionContext?.CorrelationId ?? Guid.Empty;
        RequestedBy = Guid.TryParse(executionContext?.UserContext?.UserId, out Guid id) ? id : Guid.Empty;
        Timestamp = clock.UtcNow;
    }

    public Guid MessageId { get; }

    public Guid CorrelationId { get; }

    public Guid RequestedBy { get; }

    public DateTimeOffset Timestamp { get; }
}