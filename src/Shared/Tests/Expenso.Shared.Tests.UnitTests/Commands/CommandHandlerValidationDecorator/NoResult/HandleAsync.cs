using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerValidationDecorator.NoResult;

internal sealed class HandleAsync : CommandHandlerValidationDecoratorTestBase
{
    [Test]
    public void Should_ThrowValidationException_When_ValidationErrorsOccurred()
    {
        // Arrange
        Dictionary<string, string> errors = new()
        {
            { "Id", "Id is required" },
            { "Name", "Name is required" }
        };

        _validator.Setup(expression: x => x.Validate(_testCommand)).Returns(value: errors);

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(code: () =>
            TestCandidate.HandleAsync(command: _testCommand, cancellationToken: CancellationToken.None));

        exception.Should().NotBeNull();
        exception?.ErrorDictionary.Should().NotBeNull();
        exception?.ErrorDictionary.Should().BeEquivalentTo(expectation: errors);
        string details = $"Id: Id is required{Environment.NewLine}Name: Name is required{Environment.NewLine}";
        exception?.Details.Should().NotBeNull();
        exception?.Details.Should().Be(expected: details);
    }
}