using Expenso.Shared.System.Types.Exceptions.Validation;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerValidationDecorator.Result;

internal sealed class HandleAsync : CommandHandlerDecoratorTestBase
{
    [Test]
    public async Task Should_ReturnCorrectResult_When_NoValidationErrorsOccurred()
    {
        // Arrange
        TestCommandResult expectedCommandResult = new(Message: "Test message");
        _validator.Setup(expression: x => x.Validate(_testCommand)).Returns(value: new Dictionary<string, string>());

        _handler
            .Setup(expression: x => x.HandleAsync(_testCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: expectedCommandResult);

        // Act
        TestCommandResult? commandResult =
            await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: CancellationToken.None);

        // Assert
        commandResult.Should().NotBeNull();
        commandResult.Should().Be(expected: expectedCommandResult);
    }

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