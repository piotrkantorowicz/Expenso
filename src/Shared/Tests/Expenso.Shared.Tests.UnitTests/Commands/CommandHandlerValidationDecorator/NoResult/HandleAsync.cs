using Expenso.Shared.System.Types.Exceptions.Validation;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerValidationDecorator.NoResult;

internal sealed class HandleAsync : CommandHandlerValidationDecoratorTestBase
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
            .Returns(value: new Dictionary<string, string>
            {
                { "Id", "Id is required" },
                { "Name", "Name is required" }
            });

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: CancellationToken.None);

        // Assert
        await action
            .Should()
            .ThrowAsync<ValidationException>()
            .Where(exceptionExpression: x =>
                errors.All(y => x.Errors.Contains(new ValidationDetailModel(y.Key, y.Value))))
            .Where(exceptionExpression: x => x.Details ==
                                             $"Id: Id is required{Environment.NewLine}Name: Name is required{Environment.NewLine}");
    }
}