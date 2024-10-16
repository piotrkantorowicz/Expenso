using Expenso.Shared.System.Logging;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.SettingsService;

[TestFixture]
internal sealed class Bind : SettingsServiceTestBase
{
    [Test]
    public void Should_BindSettings_When_ConfigurationSucceeds()
    {
        // Arrange
        const string sectionName = "Test";

        TestSettings settings = new()
        {
            IsEnabled = true,
            Name = "Name"
        };

        // Act
        TestSettings? result = TestCandidate.Bind(sectionName: sectionName);

        // Assert
        result.Should().Be(expected: settings);

        _loggerMock.Verify(
            expression: l => l.LogInfo(LoggingUtils.ConfigurationInformation,
                "Settings of type {SettingsType} have been successfully bound from section {SectionName}", null,
                nameof(TestSettings), sectionName), times: Times.Once);
    }

    [Test]
    public void Should_ReturnBindedSettings_When_SettingsHaveBeenBoundedBefore()
    {
        // Arrange
        const string sectionName = "Test";

        TestSettings settings = new()
        {
            IsEnabled = true,
            Name = "Name"
        };

        TestCandidate.Bind(sectionName: sectionName);

        // Act
        TestSettings? result = TestCandidate.Bind(sectionName: sectionName);

        // Assert
        result.Should().Be(expected: settings);

        _loggerMock.Verify(
            expression: l => l.LogDebug(LoggingUtils.ConfigurationInformation,
                "Settings of type {SettingsType} have already been bound. Returning the current instance", null,
                nameof(TestSettings)), times: Times.Once);
    }

    [Test]
    public void Should_ReturnNull_When_ConfigurationCannotBindSettings()
    {
        // Arrange
        // Act
        TestSettings? result = TestCandidate.Bind(sectionName: null!);

        // Assert
        result.Should().Be(expected: null);

        _loggerMock.Verify(
            expression: l => l.LogWarning(LoggingUtils.ConfigurationWarning,
                "Failed to bind settings of type {SettingsType} from section {SectionName}", null, null,
                nameof(TestSettings), null), times: Times.Once);
    }
}