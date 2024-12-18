using Expenso.Shared.Queries.Pagination.Decorators;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Queries.Pagination;

public static class Extensions
{
    public static IServiceCollection AddQueryPaging(this IServiceCollection services)
    {
        services.TryDecorate(serviceType: typeof(IQueryHandler<,>),
            decoratorType: typeof(QueryHandlerPaginationDecorator<,>));

        return services;
    }
}