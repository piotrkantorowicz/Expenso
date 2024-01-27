using System.Text;

using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;
using Expenso.Shared.Types.Exceptions;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerDecorator.Result;

internal sealed class HandleAsync : CommandHandlerDecoratorTestBase
{
    [Test]
    public async Task Should_ReturnCorrectResult_When_NoValidationErrorsOccurred()
    {
        // Arrange
        TestCommandResult expectedCommandResult = new("Test message");
        _validator.Setup(x => x.Validate(_testCommand)).Returns(new Dictionary<string, string>());

        _handler
            .Setup(x => x.HandleAsync(_testCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCommandResult);

        // Act
        TestCommandResult? commandResult = await TestCandidate.HandleAsync(_testCommand, CancellationToken.None);

        // Assert
        commandResult.Should().NotBeNull();
        commandResult.Should().Be(expectedCommandResult);
    }

    [Test]
    public void Should_ThrowValidationException_When_ValidationErrorsOccurred()
    {
        // Arrange
        Dictionary<string, string> errors = new()
        {
            { "Id", "Id is required" },
            { "Name", "Name is required" }
        };

        _validator.Setup(x => x.Validate(_testCommand)).Returns(errors);

        // Act
        // Assert
        ValidationException? exception =
            Assert.ThrowsAsync<ValidationException>(() =>
                TestCandidate.HandleAsync(_testCommand, CancellationToken.None));

        exception.Should().NotBeNull();
        exception?.ErrorDictionary.Should().NotBeNull();
        exception?.ErrorDictionary.Should().BeEquivalentTo(errors);

        string details = new StringBuilder()
            .AppendLine("Id: Id is required")
            .AppendLine("Name: Name is required")
            .ToString();

        exception?.Details.Should().NotBeNull();
        exception?.Details.Should().Be(details);
    }
}