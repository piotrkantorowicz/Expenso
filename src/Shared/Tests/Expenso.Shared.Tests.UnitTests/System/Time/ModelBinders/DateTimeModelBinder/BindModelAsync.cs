using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Request;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.ModelBinders.DateTimeModelBinder;

[TestFixture]
internal sealed class BindModelAsync : DateTimeModelBinderTestBase
{
    [Test, TestCase(arg: typeof(DateTimeOffset), TestName = "Should_BindModel_When_ValidDateTimeOffset"),
     TestCase(arg: typeof(DateTimeOffset?), TestName = "Should_BindModel_When_ValidNullableDateTimeOffset")]
    public async Task Should_BindModel_When_ValidDateTimeOffset(Type modelType)
    {
        // Arrange
        _bindingContext.ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType: modelType);

        _valueProviderMock
            .Setup(expression: v => v.GetValue(It.IsAny<string>()))
            .Returns(value: new ValueProviderResult(values: "2023-12-25T00:00:00.0000000Z"));

        // Act
        await _binder.BindModelAsync(bindingContext: _bindingContext);

        // Assert
        _bindingContext.Result.IsModelSet.Should().BeTrue();

        _bindingContext
            .Result.Model.Should()
            .BeOfType<DateTimeOffset>()
            .Which.Should()
            .Be(expected: new DateTimeOffset(year: 2023, month: 12, day: 25, hour: 0, minute: 0, second: 0,
                offset: TimeSpan.Zero));
    }

    [Test, TestCase(arg: typeof(DateTime), TestName = "Should_BindModel_When_ValidNullableDateTime"),
     TestCase(arg: typeof(DateTime?), TestName = "Should_BindModel_When_ValidNullableDateTime")]
    public async Task Should_BindModel_When_ValidDateTime(Type modelType)
    {
        // Arrange
        _bindingContext.ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType: modelType);

        _valueProviderMock
            .Setup(expression: v => v.GetValue(It.IsAny<string>()))
            .Returns(value: new ValueProviderResult(values: "2023-12-25T00:00:00"));

        // Act
        await _binder.BindModelAsync(bindingContext: _bindingContext);

        // Assert
        _bindingContext.Result.IsModelSet.Should().BeTrue();

        _bindingContext
            .Result.Model.Should()
            .BeOfType<DateTime>()
            .Which.Should()
            .Be(expected: new DateTime(year: 2023, month: 12, day: 25, hour: 0, minute: 0, second: 0,
                kind: DateTimeKind.Utc));
    }

    [Test, TestCaseSource(sourceName: nameof(ValidDateTimeCases))]
    public async Task Should_ReturnModel_When_DateTimeIsValid(string value, string timeZoneId, DateTime expected)
    {
        // Arrange
        _bindingContext.ModelMetadata =
            new EmptyModelMetadataProvider().GetMetadataForType(modelType: typeof(DateTime?));

        _valueProviderMock
            .Setup(expression: v => v.GetValue(It.IsAny<string>()))
            .Returns(value: new ValueProviderResult(values: value));

        _requestTimeZone.Setup(expression: f => f()).Returns(value: new RequestTimeZone(name: timeZoneId));

        // Act
        await _binder.BindModelAsync(bindingContext: _bindingContext);

        // Assert
        _bindingContext.Result.IsModelSet.Should().BeTrue();
        _bindingContext.Result.Model.Should().BeOfType<DateTime>().Which.Should().Be(expected: expected);
    }

    [Test, TestCaseSource(sourceName: nameof(ValidDateTimeOffsetCases))]
    public async Task Should_ReturnModel_When_DateTimeOffsetIsValid(string value, string timeZoneId,
        DateTimeOffset expected)
    {
        // Arrange
        _bindingContext.ModelMetadata =
            new EmptyModelMetadataProvider().GetMetadataForType(modelType: typeof(DateTimeOffset?));

        _valueProviderMock
            .Setup(expression: v => v.GetValue(It.IsAny<string>()))
            .Returns(value: new ValueProviderResult(values: value));

        _requestTimeZone.Setup(expression: f => f()).Returns(value: new RequestTimeZone(name: timeZoneId));

        // Act
        await _binder.BindModelAsync(bindingContext: _bindingContext);

        // Assert
        _bindingContext.Result.IsModelSet.Should().BeTrue();
        _bindingContext.Result.Model.Should().BeOfType<DateTimeOffset>().Which.Should().Be(expected: expected);
    }

    [Test, TestCase(arg: typeof(DateTime), TestName = "Should_ReturnNull_When_ValueIsEmpty"),
     TestCase(arg: typeof(DateTimeOffset), TestName = "Should_ReturnNull_When_ValueIsEmpty"),
     TestCase(arg: typeof(DateTime?), TestName = "Should_ReturnNull_When_ValueIsEmpty"),
     TestCase(arg: typeof(DateTimeOffset?), TestName = "Should_ReturnNull_When_ValueIsEmpty")]
    public async Task Should_ReturnNull_When_ValueIsEmpty(Type modelType)
    {
        // Arrange
        _bindingContext.ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType: modelType);

        _valueProviderMock
            .Setup(expression: v => v.GetValue(It.IsAny<string>()))
            .Returns(value: ValueProviderResult.None);

        // Act
        await _binder.BindModelAsync(bindingContext: _bindingContext);

        // Assert
        _bindingContext.Result.IsModelSet.Should().BeTrue();
        _bindingContext.Result.Model.Should().BeNull();
    }

    [Test, TestCase(arg: typeof(DateTime), TestName = "Should_AddModelError_When_InvalidDateTime"),
     TestCase(arg: typeof(DateTimeOffset), TestName = "Should_AddModelError_When_InvalidDateTimeOffset"),
     TestCase(arg: typeof(DateTime?), TestName = "Should_AddModelError_When_InvalidNullableDateTime"),
     TestCase(arg: typeof(DateTimeOffset?), TestName = "Should_AddModelError_When_InvalidNullableDateTimeOffset")]
    public async Task Should_AddModelError_When_InvalidDateTime(Type modelType)
    {
        // Arrange
        _bindingContext.ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType: modelType);

        _valueProviderMock
            .Setup(expression: v => v.GetValue(It.IsAny<string>()))
            .Returns(value: new ValueProviderResult(values: "invalid-date"));

        // Act
        await _binder.BindModelAsync(bindingContext: _bindingContext);

        // Assert
        _bindingContext.ModelState.Should().ContainKey(expected: "test");
        _bindingContext.ModelState[key: "test"]?.Errors.Should().NotBeEmpty();
    }

    private static IEnumerable<object> ValidDateTimeCases()
    {
        yield return new object[]
        {
            "2024-03-10T14:00:00",
            TimeZoneIds.Utc,
            new DateTime(year: 2024, month: 3, day: 10, hour: 14, minute: 0, second: 0, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "2024-03-10T19:00:00",
            "Pacific/Honolulu",
            new DateTime(year: 2024, month: 3, day: 11, hour: 5, minute: 0, second: 0, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "2023-12-25T00:00:00",
            "America/Los_Angeles",
            new DateTime(year: 2023, month: 12, day: 25, hour: 8, minute: 0, second: 0, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "2022-01-01T12:30:45",
            "Australia/Brisbane",
            new DateTime(year: 2022, month: 1, day: 1, hour: 02, minute: 30, second: 45, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "2021-07-04T18:45:00",
            "Asia/Baku",
            new DateTime(year: 2021, month: 7, day: 4, hour: 14, minute: 45, second: 0, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "2020-02-29T23:59:59",
            "Atlantic/Reykjavik",
            new DateTime(year: 2020, month: 2, day: 29, hour: 23, minute: 59, second: 59, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "9999-12-31T23:59:59",
            TimeZoneIds.Utc,
            new DateTime(year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "0001-01-01T00:00:00",
            TimeZoneIds.Utc,
            new DateTime(year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc)
        };
    }

    private static IEnumerable<object> ValidDateTimeOffsetCases()
    {
        yield return new object[]
        {
            "2024-03-10T14:00:00.0000000Z",
            TimeZoneIds.Utc,
            new DateTimeOffset(dateTime: new DateTime(year: 2024, month: 3, day: 10, hour: 14, minute: 0, second: 0,
                kind: DateTimeKind.Utc))
        };

        yield return new object[]
        {
            "2024-03-10T19:00:00.0000000-10:00",
            "Pacific/Honolulu",
            new DateTimeOffset(dateTime: new DateTime(year: 2024, month: 3, day: 11, hour: 5, minute: 0, second: 0,
                kind: DateTimeKind.Utc))
        };

        yield return new object[]
        {
            "2023-12-25T00:00:00.0000000-08:00",
            "America/Los_Angeles",
            new DateTimeOffset(dateTime: new DateTime(year: 2023, month: 12, day: 25, hour: 8, minute: 0, second: 0,
                kind: DateTimeKind.Utc))
        };

        yield return new object[]
        {
            "2022-01-01T12:30:45.0000000+10:00",
            "Australia/Brisbane",
            new DateTimeOffset(dateTime: new DateTime(year: 2022, month: 1, day: 1, hour: 02, minute: 30, second: 45,
                kind: DateTimeKind.Utc))
        };

        yield return new object[]
        {
            "2021-07-04T18:45:00.0000000+04:00",
            "Asia/Baku",
            new DateTimeOffset(dateTime: new DateTime(year: 2021, month: 7, day: 4, hour: 14, minute: 45, second: 0,
                kind: DateTimeKind.Utc))
        };

        yield return new object[]
        {
            "2020-02-29T23:59:59.0000000Z",
            "Atlantic/Reykjavik",
            new DateTimeOffset(dateTime: new DateTime(year: 2020, month: 2, day: 29, hour: 23, minute: 59, second: 59,
                kind: DateTimeKind.Utc))
        };

        yield return new object[]
        {
            "9999-12-31T23:59:59.0000000+00:00",
            TimeZoneIds.Utc,
            new DateTimeOffset(dateTime: new DateTime(year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59,
                kind: DateTimeKind.Utc))
        };

        yield return new object[]
        {
            "0001-01-01T00:00:00.0000000+00:00",
            TimeZoneIds.Utc,
            new DateTimeOffset(dateTime: new DateTime(year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0,
                kind: DateTimeKind.Utc))
        };
    }
}