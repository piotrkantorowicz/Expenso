using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Domain.Shared;

public static class MessageContextFactoryResolver
{
    private static IServiceProvider? _serviceProvider;
    private static bool _isInitialized;

    public static void BindResolver(IServiceProvider? serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(paramName: nameof(serviceProvider));
        _isInitialized = true;
    }

    internal static IMessageContextFactory Resolve()
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException(message: "MessageContextFactoryResolver is not initialized");
        }

        return _serviceProvider!.GetRequiredService<IMessageContextFactory>();
    }
}