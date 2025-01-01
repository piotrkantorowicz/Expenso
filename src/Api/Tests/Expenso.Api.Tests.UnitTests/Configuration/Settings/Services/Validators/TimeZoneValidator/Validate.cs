using Expenso.Api.Configuration.Settings.ApiSettings.TimeZone;
using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

using NUnit.Framework;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.TimeZoneValidator;

internal sealed class Validate : TimeZoneSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnNoErrors_When_TimeZoneSettingsIsValid()
    {
        // Act
        ValidationResult? validationResult = TestCandidate.Validate(instance: _timeZoneSettings);

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test]
    public void Should_ReturnError_When_EnableRequestToUtcIsNull()
    {
        // Arrange
        _timeZoneSettings = _timeZoneSettings with
        {
            EnableRequestToUtc = null
        };

        // Act
        ValidationResult? validationResult = TestCandidate.Validate(instance: _timeZoneSettings);

        // Assert
        validationResult.AssertIsSingleError(propertyName: nameof(_timeZoneSettings.EnableRequestToUtc),
            errorMessage: "EnableRequestToUtc must be provided.");
    }

    [Test]
    public void Should_ReturnError_When_EnableResponseToLocalIsNull()
    {
        // Arrange
        _timeZoneSettings = _timeZoneSettings with
        {
            EnableResponseToLocal = null
        };

        // Act
        ValidationResult? validationResult = TestCandidate.Validate(instance: _timeZoneSettings);

        // Assert
        validationResult.AssertIsSingleError(propertyName: nameof(_timeZoneSettings.EnableResponseToLocal),
            errorMessage: "EnableResponseToLocal must be provided.");
    }

    [Test]
    public void Should_ReturnNoErrors_When_SupportedDateTimeFormatsIsNull()
    {
        // Arrange
        _timeZoneSettings = _timeZoneSettings with
        {
            SupportedDateTimeFormats = null
        };

        // Act
        ValidationResult? validationResult = TestCandidate.Validate(instance: _timeZoneSettings);

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test]
    public void Should_ReturnError_When_SupportedDateTimeFormatsNotMatch()
    {
        // Arrange
        _timeZoneSettings = _timeZoneSettings with
        {
            SupportedDateTimeFormats = ["InvalidDateTimeFormat"]
        };

        // Act
        ValidationResult? validationResult = TestCandidate.Validate(instance: _timeZoneSettings);

        // Assert
        validationResult.AssertIsSingleError(propertyName: nameof(_timeZoneSettings.SupportedDateTimeFormats),
            errorMessage:
            $"Supported date formats must be provided and must be one of the following: {string.Join(separator: ", ", value: DateTimeFormats.SupportedDateTimeFormats)}");
    }

    [Test]
    public void Should_ReturnNoErrors_When_SupportedDateTimeOffsetFormatsIsNull()
    {
        // Arrange
        _timeZoneSettings = _timeZoneSettings with
        {
            SupportedDateTimeOffsetFormats = null
        };

        // Act
        ValidationResult? validationResult = TestCandidate.Validate(instance: _timeZoneSettings);

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test]
    public void Should_ReturnError_When_SupportedDateTimeOffsetFormatsNotMatch()
    {
        // Arrange
        _timeZoneSettings = _timeZoneSettings with
        {
            SupportedDateTimeOffsetFormats = ["InvalidDateTimeOffsetFormat"]
        };

        // Act
        ValidationResult? validationResult = TestCandidate.Validate(instance: _timeZoneSettings);

        // Assert
        validationResult.AssertIsSingleError(propertyName: nameof(_timeZoneSettings.SupportedDateTimeOffsetFormats),
            errorMessage:
            $"Supported date time offset formats must be provided and must be one of the following: {string.Join(separator: ", ", value: DateTimeFormats.SupportedDateTimeOffsetFormats)}");
    }

    [Test]
    public void Should_ReturnError_When_TimeZoneProviderTypeIsNone()
    {
        // Arrange
        _timeZoneSettings = _timeZoneSettings with
        {
            TimeZoneProviderType = TimeZoneProviderType.None
        };

        // Act
        ValidationResult? validationResult = TestCandidate.Validate(instance: _timeZoneSettings);

        // Assert
        validationResult.AssertIsSingleError(propertyName: nameof(_timeZoneSettings.TimeZoneProviderType),
            errorMessage:
            "At least one request time zone provider must be provided when request to UTC or response to local is enabled.");
    }
}