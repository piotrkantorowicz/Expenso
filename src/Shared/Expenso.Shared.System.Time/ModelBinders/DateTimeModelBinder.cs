using System.Globalization;

using Expenso.Shared.System.Time.Request;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Expenso.Shared.System.Time.ModelBinders;

internal sealed class DateTimeModelBinder : IModelBinder
{
    private readonly Func<RequestTimeZone> _requestTimeZone;
    private readonly string[] _dateTimeSupportedFormats;
    private readonly string[] _dateTimeOffsetSupportedFormats;

    public DateTimeModelBinder(Func<RequestTimeZone> requestTimeZone, string[] dateTimeSupportedFormats,
        string[] dateTimeOffsetSupportedFormats)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));

        _dateTimeSupportedFormats = dateTimeSupportedFormats ??
                                    throw new ArgumentNullException(paramName: nameof(dateTimeSupportedFormats));

        _dateTimeOffsetSupportedFormats = dateTimeOffsetSupportedFormats ??
                                          throw new ArgumentNullException(
                                              paramName: nameof(dateTimeOffsetSupportedFormats));
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(argument: bindingContext);
        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(key: bindingContext.ModelName);

        if (string.IsNullOrEmpty(value: valueProviderResult.FirstValue))
        {
            bindingContext.Result = ModelBindingResult.Success(model: null);
        }
        else
        {
            Type modelType = bindingContext.ModelType;
            TimeZoneInfo timeZone = _requestTimeZone().TimeZone;

            if (modelType == typeof(DateTimeOffset?) || modelType == typeof(DateTimeOffset))
            {
                if (DateTimeOffset.TryParseExact(input: valueProviderResult.FirstValue,
                        format: _dateTimeOffsetSupportedFormats[0], formatProvider: CultureInfo.InvariantCulture,
                        styles: DateTimeStyles.None, result: out DateTimeOffset dateTimeOffset))
                {
                    DateTimeOffset dateTimeUtc = TimeZoneInfo.ConvertTime(dateTimeOffset: dateTimeOffset,
                        destinationTimeZone: timeZone);

                    bindingContext.Result = ModelBindingResult.Success(model: dateTimeUtc);
                }
                else
                {
                    AddModelError(bindingContext: bindingContext, valueProviderResult: valueProviderResult,
                        modelTypeName: nameof(DateTimeOffset));
                }
            }
            else if (modelType == typeof(DateTime?) || modelType == typeof(DateTime))
            {
                if (DateTime.TryParseExact(s: valueProviderResult.FirstValue, format: _dateTimeSupportedFormats[0],
                        provider: CultureInfo.InvariantCulture, style: DateTimeStyles.None,
                        result: out DateTime dateTime))
                {
                    DateTime dateTimeUtc = TimeZoneInfo.ConvertTime(dateTime: dateTime, sourceTimeZone: timeZone,
                        destinationTimeZone: TimeZoneInfo.Utc);

                    bindingContext.Result = ModelBindingResult.Success(model: dateTimeUtc);
                }
                else
                {
                    AddModelError(bindingContext: bindingContext, valueProviderResult: valueProviderResult,
                        modelTypeName: nameof(DateTime));
                }
            }
        }

        return Task.CompletedTask;
    }

    private static void AddModelError(ModelBindingContext bindingContext, ValueProviderResult valueProviderResult,
        string modelTypeName)
    {
        string invalidAccessor =
            bindingContext.ModelMetadata.ModelBindingMessageProvider.AttemptedValueIsInvalidAccessor(
                arg1: valueProviderResult.ToString(), arg2: modelTypeName);

        bindingContext.ModelState.TryAddModelError(key: bindingContext.ModelName, errorMessage: invalidAccessor);
    }
}