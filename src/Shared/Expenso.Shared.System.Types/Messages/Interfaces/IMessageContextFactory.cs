namespace Expenso.Shared.System.Types.Messages.Interfaces;

public interface IMessageContextFactory
{
    IMessageContext Current(Guid? messageId = null, string? moduleId = null);

    IMessageContext FromParent(IMessageContext parent, string? moduleId, Guid? messageId = null);
}