using Expenso.Shared.System.Types.Pagination;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Expenso.Api.Configuration.Binders;

internal sealed class PaginationModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(argument: context);
        Type? modelType = context.Metadata.UnderlyingOrModelType;
        Type? underlyingType = Nullable.GetUnderlyingType(nullableType: modelType) ?? modelType;

        return underlyingType == typeof(Paging)
            ? new BinderTypeModelBinder(binderType: typeof(PaginationModelBinder))
            : null;
    }
}