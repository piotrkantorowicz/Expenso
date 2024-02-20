using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Types.Messages;

internal sealed class MessageContextFactory(IServiceProvider serviceProvider) : IMessageContextFactory
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public IMessageContext Current(Guid? messageId = null)
    {
        IExecutionContextAccessor executionContextAccessor =
            _serviceProvider.GetRequiredService<IExecutionContextAccessor>();

        IClock clock = _serviceProvider.GetRequiredService<IClock>();

        return new MessageContext(executionContextAccessor.Get(), clock, messageId);
    }
}