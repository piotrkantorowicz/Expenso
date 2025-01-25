using Expenso.Shared.System.Types.Exceptions;

using FluentValidation.Results;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerValidationDecorator.NoResult;

[TestFixture]
internal sealed class HandleAsync : CommandHandlerValidationDecoratorTestBase
{
    [Test]
    public async Task Should_ThrowValidationException_When_ValidationErrorsOccurred()
    {
        // Arrange
        Dictionary<string, string> errors = new()
        {
            { "Id", "ID is required" },
            { "Name", "Name is required" }
        };

        _validator
            .Setup(expression: x => x.Validate(_testCommand))
            .Returns(value: new ValidationResult(failures:
            [
                new ValidationFailure(propertyName: "Id", errorMessage: "ID is required"),
                new ValidationFailure(propertyName: "Name", errorMessage: "Name is required")
            ]));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: default);

        // Assert
        ValidationException? exception = await action.ShouldThrowAsync<ValidationException>();
        exception.ErrorDictionary.ShouldBe(expected: errors);

        exception.Details.ShouldBe(
            expected: $"Id: ID is required{Environment.NewLine}Name: Name is required{Environment.NewLine}");
    }
}