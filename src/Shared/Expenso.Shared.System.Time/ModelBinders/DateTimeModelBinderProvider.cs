using Expenso.Shared.System.Time.Request;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Expenso.Shared.System.Time.ModelBinders;

internal sealed class DateTimeModelBinderProvider : IModelBinderProvider
{
    private readonly Func<RequestTimeZone> _requestTimeZone;

    public DateTimeModelBinderProvider(Func<RequestTimeZone> requestTimeZone)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));
    }

    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(argument: context);

        if (context.Metadata.UnderlyingOrModelType == typeof(DateTimeOffset?) ||
            context.Metadata.UnderlyingOrModelType == typeof(DateTimeOffset) ||
            context.Metadata.UnderlyingOrModelType == typeof(DateTime?) ||
            context.Metadata.UnderlyingOrModelType == typeof(DateTime))
        {
            return new DateTimeModelBinder(requestTimeZone: _requestTimeZone);
        }

        return null;
    }
}