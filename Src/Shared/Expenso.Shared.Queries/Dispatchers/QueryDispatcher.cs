using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Queries.Dispatchers;

internal sealed class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task<TResult?> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        where TResult : class
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        MethodInfo? method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync));

        if (method is null)
        {
            throw new InvalidOperationException($"Query handler for '{typeof(TResult).Name}' is invalid.");
        }

        object? handler = scope.ServiceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new InvalidOperationException($"Query handler for '{typeof(TResult).Name}' not found.");
        }

        return await (Task<TResult?>)method.Invoke(handler, [
            query,
            cancellationToken
        ])!;
    }
}