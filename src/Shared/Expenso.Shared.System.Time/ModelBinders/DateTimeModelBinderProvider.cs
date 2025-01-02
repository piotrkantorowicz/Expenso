using Expenso.Shared.System.Time.Request;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Expenso.Shared.System.Time.ModelBinders;

internal sealed class DateTimeModelBinderProvider : IModelBinderProvider
{
    private readonly Func<RequestTimeZone> _requestTimeZone;
    private readonly string[] _dateTimeSupportedFormats;
    private readonly string[] _dateTimeOffsetSupportedFormats;

    public DateTimeModelBinderProvider(Func<RequestTimeZone> requestTimeZone, string[] dateTimeSupportedFormats,
        string[] dateTimeOffsetSupportedFormats)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));

        _dateTimeSupportedFormats = dateTimeSupportedFormats ??
                                    throw new ArgumentNullException(paramName: nameof(dateTimeSupportedFormats));

        _dateTimeOffsetSupportedFormats = dateTimeOffsetSupportedFormats ??
                                          throw new ArgumentNullException(
                                              paramName: nameof(dateTimeOffsetSupportedFormats));
    }

    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(argument: context);

        if (context.Metadata.UnderlyingOrModelType == typeof(DateTimeOffset?) ||
            context.Metadata.UnderlyingOrModelType == typeof(DateTimeOffset) ||
            context.Metadata.UnderlyingOrModelType == typeof(DateTime?) ||
            context.Metadata.UnderlyingOrModelType == typeof(DateTime))
        {
            return new DateTimeModelBinder(requestTimeZone: _requestTimeZone,
                dateTimeSupportedFormats: _dateTimeSupportedFormats,
                dateTimeOffsetSupportedFormats: _dateTimeOffsetSupportedFormats);
        }

        return null;
    }
}