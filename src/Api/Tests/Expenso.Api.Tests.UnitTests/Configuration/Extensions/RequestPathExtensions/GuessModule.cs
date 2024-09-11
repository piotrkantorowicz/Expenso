using Expenso.Api.Configuration.Extensions;
using Expenso.BudgetSharing.Api;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Api.Tests.UnitTests.Configuration.Extensions.RequestPathExtensions;

internal sealed class GuessModule : RequestPathExtensionTestBase
{
    [Test]
    public void GuessModule_ShouldReturnNull_WhenRequestPathIsNull()
    {
        // Act
        string? result = ((string?)null).GuessModule(logger: _loggerMock.Object);

        // Assert
        result.Should().BeNull();

        _loggerMock.Verify(
            expression: logger => logger.LogWarning(LoggingUtils.GeneralWarning, "Request path is null or empty",
                It.IsAny<Exception>(), It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Never);
    }

    [Test]
    public void GuessModule_ShouldReturnNull_WhenRequestPathContainsManagementPath()
    {
        // Arrange
        const string requestPath = "/health";

        // Act
        string? result = requestPath.GuessModule(logger: _loggerMock.Object);

        // Assert
        result.Should().BeNull();

        _loggerMock.Verify(
            expression: logger => logger.LogDebug(LoggingUtils.GeneralInformation,
                "Management urls cannot be used to define module names", It.IsAny<IMessageContext?>(),
                It.IsAny<object?[]>()), times: Times.Once);
    }

    [Test]
    public void GuessModule_ShouldReturnNull_WhenNoRegisteredModules()
    {
        // Arrange
        const string requestPath = "/api/unknown";

        // Act
        string? result = requestPath.GuessModule(logger: _loggerMock.Object);

        // Assert
        result.Should().BeNull();

        _loggerMock.Verify(
            expression: logger => logger.LogWarning(LoggingUtils.GeneralWarning, "No registered modules found",
                It.IsAny<Exception>(), It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Once);
    }

    [Test]
    public void GuessModule_ShouldReturnModuleName_WhenMatchingPrefixIsFound()
    {
        // Arrange
        const string requestPath = "/api/budget-sharing";
        Modules.RegisterModule<BudgetSharingModule>();

        // Act
        string? result = requestPath.GuessModule(logger: _loggerMock.Object);

        // Assert
        result.Should().Be(expected: "BudgetSharingModule");

        _loggerMock.Verify(
            expression: logger => logger.LogDebug(LoggingUtils.GeneralInformation,
                It.Is<string>(message =>
                    message.Contains($"Module found: {nameof(BudgetSharingModule)} for request path: {requestPath}")),
                It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Once);
    }

    [Test]
    public void GuessModule_ShouldReturnNull_WhenNoMatchingPrefixIsFound()
    {
        // Arrange
        const string requestPath = "/api/unknown";
        Modules.RegisterModule<BudgetSharingModule>();

        // Act
        string? result = requestPath.GuessModule(logger: _loggerMock.Object);

        // Assert
        result.Should().BeNull();

        _loggerMock.Verify(
            expression: logger => logger.LogWarning(LoggingUtils.GeneralWarning,
                $"No matching module found for request path: {requestPath}", It.IsAny<Exception>(),
                It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Once);
    }
}