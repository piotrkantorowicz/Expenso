using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Queries.Dispatchers;

internal sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(paramName: nameof(serviceProvider));
    }

    public async Task<TResult?> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        where TResult : class
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        MethodInfo? method = handlerType.GetMethod(name: nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync));

        if (method is null)
        {
            throw new InvalidOperationException(message: $"Query handler for '{typeof(TResult).Name}' is invalid");
        }

        object? handler = scope.ServiceProvider.GetService(serviceType: handlerType);

        if (handler is null)
        {
            throw new InvalidOperationException(message: $"Query handler for '{typeof(TResult).Name}' not found");
        }

        return await (Task<TResult?>)method.Invoke(obj: handler, parameters:
        [
            query,
            cancellationToken
        ])!;
    }
}