using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Types.Messages;

internal sealed class MessageContextFactory : IMessageContextFactory
{
    private readonly IServiceProvider _serviceProvider;

    public MessageContextFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(paramName: nameof(serviceProvider));
    }

    public IMessageContext Current(Guid? messageId = null)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        IExecutionContextAccessor executionContextAccessor =
            scope.ServiceProvider.GetRequiredService<IExecutionContextAccessor>();

        IClock clock = scope.ServiceProvider.GetRequiredService<IClock>();

        return new MessageContext(executionContext: executionContextAccessor.Get(), clock: clock, messageId: messageId);
    }
}