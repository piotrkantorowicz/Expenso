using Expenso.Api.Configuration.Settings.ApiSettings.TimeZone;
using Expenso.Shared.System.Time.Constants;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class TimeZoneSettingsValidator : AbstractValidator<TimeZoneSettings>
{
    public TimeZoneSettingsValidator()
    {
        RuleFor(expression: x => x.EnableRequestToUtc)
            .NotNull()
            .WithMessage(errorMessage: "EnableRequestToUtc must be provided.");

        RuleFor(expression: x => x.EnableResponseToLocal)
            .NotNull()
            .WithMessage(errorMessage: "EnableResponseToLocal must be provided.");

        When(predicate: x => x.SupportedDateTimeFormats is not null, action: () =>
            RuleFor(expression: x => x.SupportedDateTimeFormats)
                .Must(predicate: x =>
                    x is { Length: > 0 } &&
                    x.All(predicate: y => DateTimeFormats.SupportedDateTimeFormats.Contains(value: y)))
                .WithMessage(
                    errorMessage:
                    $"Supported date formats must be provided and must be one of the following: {string.Join(separator: ", ", value: DateTimeFormats.SupportedDateTimeFormats)}"));

        When(predicate: x => x.SupportedDateTimeOffsetFormats is not null, action: () =>
            RuleFor(expression: x => x.SupportedDateTimeOffsetFormats)
                .Must(predicate: x => x is { Length: > 0 } &&
                                      x.All(predicate: y =>
                                          DateTimeFormats.SupportedDateTimeOffsetFormats.Contains(value: y)))
                .WithMessage(
                    errorMessage:
                    $"Supported date time offset formats must be provided and must be one of the following: {string.Join(separator: ", ", value: DateTimeFormats.SupportedDateTimeOffsetFormats)}"));

        When(predicate: x => x.EnableRequestToUtc is true || x.EnableResponseToLocal is true, action: () =>
        {
            const string message =
                "At least one request time zone provider must be provided when request to UTC or response to local is enabled.";

            RuleFor(expression: x => x.TimeZoneProviderType)
                .NotNull()
                .WithMessage(errorMessage: message)
                .IsInEnum()
                .WithMessage(errorMessage: message)
                .Must(predicate: x => x != TimeZoneProviderType.None)
                .WithMessage(errorMessage: message);
        });
    }
}