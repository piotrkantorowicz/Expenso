using Expenso.Shared.System.Types.Exceptions;

using FluentValidation.Results;

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
            .Returns(value: new ValidationResult(new List<ValidationFailure>
            {
                new(propertyName: "Id", errorMessage: "ID is required"),
                new(propertyName: "Name", errorMessage: "Name is required")
            }));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: CancellationToken.None);

        // Assert
        await action
            .Should()
            .ThrowAsync<ValidationException>()
            .Where(exceptionExpression: x => errors.All(y => x.ErrorDictionary.Contains(y)))
            .Where(exceptionExpression: x => x.Details ==
                                             $"Id: ID is required{Environment.NewLine}Name: Name is required{Environment.NewLine}");
    }
}