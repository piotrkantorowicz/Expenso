using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.System.Types.Messages;

public sealed record MessageContext : IMessageContext
{
    /* Public constructors and init setters are required by the JSON serializer. */
    public MessageContext()
    {
    }

    internal MessageContext(Guid messageId, Guid correlationId, Guid requestedBy, DateTimeOffset timestamp,
        string module)
    {
        MessageId = messageId;
        CorrelationId = correlationId;
        RequestedBy = requestedBy;
        Timestamp = timestamp;
        ModuleId = module;
    }

    public string? ModuleId { get; init; }

    public Guid MessageId { get; init; }

    public Guid CorrelationId { get; init; }

    public Guid RequestedBy { get; init; }

    public DateTimeOffset Timestamp { get; init; }
}