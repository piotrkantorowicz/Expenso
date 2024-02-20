using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Domain.Shared;

public static class DependencyResolver
{
    private static IServiceProvider? _serviceProvider;
    private static bool _isInitialized;

    public static void BindResolver(IServiceProvider? serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _isInitialized = true;
    }

    internal static T Resolve<T>() where T : class
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException("DependencyResolver is not initialized");
        }

        return _serviceProvider!.GetRequiredService<T>();
    }
}