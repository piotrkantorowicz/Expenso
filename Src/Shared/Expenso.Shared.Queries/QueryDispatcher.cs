using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Queries;

internal sealed class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task<TResult?> QueryAsync<TResult>(IQuery<TResult> query,
        CancellationToken cancellationToken = default) where TResult : class
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        object handler = scope.ServiceProvider.GetRequiredService(handlerType);
        MethodInfo? method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync));

        if (method is null)
        {
            throw new InvalidOperationException($"Query handler for '{typeof(TResult).Name}' is invalid.");
        }

        return await (Task<TResult?>)method.Invoke(handler, [
            query,
            cancellationToken
        ])!;
    }
}