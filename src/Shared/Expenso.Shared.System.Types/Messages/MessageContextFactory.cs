using Expenso.Shared.System.Time;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.System.Types.ExecutionContext.Models;
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

    public IMessageContext Current(Guid? messageId = null, string? moduleId = null)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        IExecutionContextAccessor executionContextAccessor =
            scope.ServiceProvider.GetRequiredService<IExecutionContextAccessor>();

        IClock clock = scope.ServiceProvider.GetRequiredService<IClock>();
        IExecutionContext? executionContext = executionContextAccessor.Get();

        Guid requestedBy = Guid.TryParse(input: executionContext?.UserContext?.UserId, result: out Guid id)
            ? id
            : Guid.Empty;

        string executionContextModuleId = !string.IsNullOrWhiteSpace(value: executionContext?.ModuleId)
            ? executionContext.ModuleId
            : "Unknown";

        string resolvedModuleId = !string.IsNullOrWhiteSpace(value: moduleId) ? moduleId : executionContextModuleId;

        return new MessageContext(messageId: messageId ?? Guid.NewGuid(),
            correlationId: executionContext?.CorrelationId ?? Guid.Empty, requestedBy: requestedBy,
            timestamp: clock.UtcNow, module: resolvedModuleId);
    }

    public IMessageContext FromParent(IMessageContext? parent, string? moduleId, Guid? messageId = null)
    {
        if (parent is null)
        {
            return Current(messageId: messageId, moduleId: moduleId);
        }

        using IServiceScope scope = _serviceProvider.CreateScope();
        IClock clock = scope.ServiceProvider.GetRequiredService<IClock>();

        return new MessageContext(messageId: messageId ?? Guid.NewGuid(), correlationId: parent.CorrelationId,
            requestedBy: parent.RequestedBy, timestamp: clock.UtcNow,
            module: string.IsNullOrWhiteSpace(value: moduleId) ? "Unknown" : moduleId);
    }
}