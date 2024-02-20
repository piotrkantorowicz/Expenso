namespace Expenso.Shared.System.Types.Messages.Interfaces;

public interface IMessageContextFactory
{
    IMessageContext Current(Guid? messageId = null);
}