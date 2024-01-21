using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.GeneralPreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.Preferences.Cases;

internal sealed class Update : PreferenceTestBase
{
    [Test]
    public void Should_ReturnTrue_When_PropertiesChanged()
    {
        // Arrange
        GeneralPreference generalPreference = GeneralPreference.Create(true);
        NotificationPreference notificationPreference = NotificationPreference.Create(false, 0);
        FinancePreference financePreference = FinancePreference.Create(true, 2, true, 5);

        // Act
        bool result = TestCandidate.Update(generalPreference, financePreference, notificationPreference,
            _messageBrokerMock.Object, default);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_PropertiesUnchanged()
    {
        // Arrange
        // Act
        bool result = TestCandidate.Update(_defaultGeneralPreference, _defaultFinancePreference,
            _defaultNotificationPreference, _messageBrokerMock.Object, default);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_SetCorrectValues_When_PropertiesChanged()
    {
        // Arrange
        GeneralPreference generalPreference = GeneralPreference.Create(true);
        NotificationPreference notificationPreference = NotificationPreference.Create(false, 0);
        FinancePreference financePreference = FinancePreference.Create(true, 2, true, 5);

        // Act
        TestCandidate.Update(generalPreference, financePreference, notificationPreference, _messageBrokerMock.Object,
            default);

        // Assert
        TestCandidate.GeneralPreference.Should().Be(generalPreference);
        TestCandidate.FinancePreference.Should().Be(financePreference);
        TestCandidate.NotificationPreference.Should().Be(notificationPreference);
    }

    [Test]
    public void Should_RemainOldValues_When_PropertiesUnchanged()
    {
        // Arrange
        // Act
        TestCandidate.Update(_defaultGeneralPreference, _defaultFinancePreference, _defaultNotificationPreference,
            _messageBrokerMock.Object, default);

        // Assert
        TestCandidate.GeneralPreference.Should().Be(_defaultGeneralPreference);
        TestCandidate.FinancePreference.Should().Be(_defaultFinancePreference);
        TestCandidate.NotificationPreference.Should().Be(_defaultNotificationPreference);
    }

    [Test]
    public void Should_CallMessageBroker_When_PropertiesChanged()
    {
        // Arrange
        GeneralPreference generalPreference = GeneralPreference.Create(true);
        NotificationPreference notificationPreference = NotificationPreference.Create(false, 0);
        FinancePreference financePreference = FinancePreference.Create(true, 2, true, 5);

        // Act
        TestCandidate.Update(generalPreference, financePreference, notificationPreference, _messageBrokerMock.Object,
            default);

        // Assert
        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<GeneralPreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<FinancePreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Once());

        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<NotificationPreferenceUpdatedIntegrationEvent>(),
                It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public void Should_NotCallMessageBroker_When_PropertiesUnchanged()
    {
        // Arrange
        // Act
        TestCandidate.Update(_defaultGeneralPreference, _defaultFinancePreference, _defaultNotificationPreference,
            _messageBrokerMock.Object, default);

        // Assert
        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<GeneralPreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<FinancePreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Never());

        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<NotificationPreferenceUpdatedIntegrationEvent>(),
                It.IsAny<CancellationToken>()), Times.Never());
    }
}