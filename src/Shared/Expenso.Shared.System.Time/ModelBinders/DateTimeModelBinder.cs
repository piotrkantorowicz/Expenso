using System.Globalization;

using Expenso.Shared.System.Time.Request;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Expenso.Shared.System.Time.ModelBinders;

internal sealed class DateTimeModelBinder : IModelBinder
{
    private readonly Func<RequestTimeZone> _requestTimeZone;

    public DateTimeModelBinder(Func<RequestTimeZone> requestTimeZone)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));
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
            Type? modelType = bindingContext.ModelType;
            TimeZoneInfo? timeZone = _requestTimeZone().TimeZone;

            if (modelType == typeof(DateTimeOffset?) || modelType == typeof(DateTimeOffset))
            {
                if (DateTimeOffset.TryParse(input: valueProviderResult.FirstValue, formatProvider: null,
                        styles: DateTimeStyles.AdjustToUniversal, result: out DateTimeOffset parsedDateTimeOffset))
                {
                    DateTimeOffset dateTimeUtc = TimeZoneInfo
                        .ConvertTime(dateTimeOffset: parsedDateTimeOffset, destinationTimeZone: timeZone)
                        .ToUniversalTime();

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
                if (DateTime.TryParse(s: valueProviderResult.FirstValue, provider: null,
                        styles: DateTimeStyles.AdjustToUniversal, result: out DateTime parsedDateTime))
                {
                    DateTime dateTimeUtc = TimeZoneInfo
                        .ConvertTime(dateTime: parsedDateTime, destinationTimeZone: timeZone)
                        .ToUniversalTime();

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
        string? invalidAccessor =
            bindingContext.ModelMetadata.ModelBindingMessageProvider.AttemptedValueIsInvalidAccessor(
                arg1: valueProviderResult.ToString(), arg2: modelTypeName);

        bindingContext.ModelState.TryAddModelError(key: bindingContext.ModelName, errorMessage: invalidAccessor);
    }
}