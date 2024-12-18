using Expenso.Shared.System.Types.Pagination;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Expenso.Api.Configuration.Binders;

internal sealed class PaginationModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(argument: bindingContext);
        string? pageValue = bindingContext.ValueProvider.GetValue(key: "page").FirstValue;
        string? limitValue = bindingContext.ValueProvider.GetValue(key: "limit").FirstValue;

        if (int.TryParse(s: pageValue, result: out int page) && int.TryParse(s: limitValue, result: out int limit))
        {
            Paging paging = new(page: page, limit: limit);
            bindingContext.Result = ModelBindingResult.Success(model: paging);
        }
        else
        {
            bindingContext.ModelState.AddModelError(key: bindingContext.ModelName,
                errorMessage: "Invalid paging parameters");
        }

        return Task.CompletedTask;
    }
}