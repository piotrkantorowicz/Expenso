using Expenso.Shared.System.Types.Exceptions;

using FluentValidation.Results;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerFluentValidationDecorator.NoResult;

[TestFixture]
internal sealed class HandleAsync : CommandHandlerFluentValidationDecoratorTestBase
{
    [Test]
    public async Task Should_ThrowValidationException_When_ValidationErrorsOccurred()
    {
        // Arrange
        Dictionary<string, string> errors = new()
        {
            { "Id", "Id is required" },
            { "Name", "Name is required" }
        };

        _validator
            .Setup(expression: x => x.Validate(_testCommand))
            .Returns(value: new ValidationResult(failures:
            [
                new ValidationFailure(propertyName: "Id", errorMessage: "Id is required"),
                new ValidationFailure(propertyName: "Name", errorMessage: "Name is required")
            ]));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: CancellationToken.None);

        // Assert
        await action
            .Should()
            .ThrowAsync<ValidationException>()
            .Where(exceptionExpression: x => errors.All(y => x.ErrorDictionary.Contains(y)))
            .Where(exceptionExpression: x => x.Details ==
                                             $"Id: Id is required{Environment.NewLine}Name: Name is required{Environment.NewLine}");
    }
}