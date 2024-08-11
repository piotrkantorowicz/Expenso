namespace Expenso.Shared.System.Types.Messages.Interfaces;

public interface IMessageContext
{
    Guid MessageId { get; }

    Guid CorrelationId { get; }

    Guid RequestedBy { get; }

    DateTimeOffset Timestamp { get; }
}