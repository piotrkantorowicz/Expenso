namespace Expenso.Shared.System.Types.Messages.Interfaces;

public interface IMessageContext
{
    string ModuleId { get; }

    Guid MessageId { get; }

    Guid CorrelationId { get; }

    Guid RequestedBy { get; }

    DateTimeOffset Timestamp { get; }
}