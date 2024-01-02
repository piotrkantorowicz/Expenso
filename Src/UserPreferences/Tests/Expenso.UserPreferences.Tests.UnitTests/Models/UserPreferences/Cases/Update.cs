using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Proxy.IntegrationEvents;

namespace Expenso.UserPreferences.Tests.UnitTests.Models.UserPreferences.Cases;

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
            MessageBrokerMock!.Object, default);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_PropertiesUnchanged()
    {
        // Arrange

        // Act
        bool result = TestCandidate.Update(DefaultGeneralPreference, DefaultFinancePreference,
            DefaultNotificationPreference, MessageBrokerMock!.Object, default);

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
        TestCandidate.Update(generalPreference, financePreference, notificationPreference, MessageBrokerMock!.Object,
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
        TestCandidate.Update(DefaultGeneralPreference, DefaultFinancePreference, DefaultNotificationPreference,
            MessageBrokerMock!.Object, default);

        // Assert
        TestCandidate.GeneralPreference.Should().Be(DefaultGeneralPreference);
        TestCandidate.FinancePreference.Should().Be(DefaultFinancePreference);
        TestCandidate.NotificationPreference.Should().Be(DefaultNotificationPreference);
    }

    [Test]
    public void Should_CallMessageBroker_When_PropertiesChanged()
    {
        // Arrange
        GeneralPreference generalPreference = GeneralPreference.Create(true);
        NotificationPreference notificationPreference = NotificationPreference.Create(false, 0);
        FinancePreference financePreference = FinancePreference.Create(true, 2, true, 5);

        // Act
        TestCandidate.Update(generalPreference, financePreference, notificationPreference, MessageBrokerMock!.Object,
            default);

        // Assert
        MessageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<GeneralPreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);

        MessageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<FinancePreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Once());

        MessageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<NotificationPreferenceUpdatedIntegrationEvent>(),
                It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public void ShouldNot_CallMessageBroker_When_PropertiesUnchanged()
    {
        // Arrange

        // Act
        TestCandidate.Update(DefaultGeneralPreference, DefaultFinancePreference, DefaultNotificationPreference,
            MessageBrokerMock!.Object, default);

        // Assert
        MessageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<GeneralPreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);

        MessageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<FinancePreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Never());

        MessageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<NotificationPreferenceUpdatedIntegrationEvent>(),
                It.IsAny<CancellationToken>()), Times.Never());
    }
}