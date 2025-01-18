using Expenso.Shared.System.Types.Paging.Constants;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Paging.Pagination;

[TestFixture]
internal sealed class TryParse
{
    [TestCase(arguments: null, TestName = "Should_ReturnTrue_When_ValueIsNull"),
     TestCase(arg: "", TestName = "Should_ReturnTrue_When_ValueIsEmpty"),
     TestCase(arg: "InvalidValue", TestName = "Should_ReturnTrue_When_ValueIsInvalid"),
     TestCase(arg: "1", TestName = "Should_ReturnTrue_When_PartsLengthIsNotTwo"),
     TestCase(arg: "abc,25", TestName = "Should_ReturnTrue_When_PageIsNotAnInteger"),
     TestCase(arg: "1,abc", TestName = "Should_ReturnTrue_When_LimitIsNotAnInteger"),
     TestCase(arg: "0,25", TestName = "Should_ReturnTrue_When_PageIsLessThanOne"),
     TestCase(arg: "1,0", TestName = "Should_ReturnTrue_When_LimitIsLessThanOne"),
     TestCase(arg: "1,101", TestName = "Should_ReturnTrue_When_LimitExceedsMaxLimit")]
    public void Should_HandleInvalidPaginationParameters(string? value)
    {
        // Act
        bool result = Shared.System.Types.Paging.Pagination.TryParse(value: value,
            paging: out Shared.System.Types.Paging.Pagination? pagination);

        // Assert
        result.Should().BeTrue();
        pagination?.Should().NotBeNull();
        pagination?.Page.Should().Be(expected: PaginationDefaults.Page);
        pagination?.Limit.Should().Be(expected: PaginationDefaults.Limit);
    }

    [TestCase(arg1: "1,10", arg2: 1, arg3: 10, TestName = "Should_ReturnTrue_When_ValueIsValid")]
    public void Should_ReturnTrue_When_ValidValue(string value, int expectedPage, int expectedLimit)
    {
        // Act
        bool result = Shared.System.Types.Paging.Pagination.TryParse(value: value,
            paging: out Shared.System.Types.Paging.Pagination? pagination);

        // Assert
        result.Should().BeTrue();
        pagination.Should().NotBeNull();
        pagination!.Page.Should().Be(expected: expectedPage);
        pagination.Limit.Should().Be(expected: expectedLimit);
    }
}