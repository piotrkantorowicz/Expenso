using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.Preference;

internal sealed class Update : PreferenceTestBase
{
    [Test]
    public void Should_ReturnTrueForAllChangeType_When_PropertiesChanged()
    {
        // Arrange
        GeneralPreference generalPreference = GeneralPreference.Create(true);
        NotificationPreference notificationPreference = NotificationPreference.Create(false, 0);
        FinancePreference financePreference = FinancePreference.Create(true, 2, true, 5);

        // Act
        PreferenceChangeType result =
            TestCandidate.Update(generalPreference, financePreference, notificationPreference);

        // Assert
        result.Should().BeEquivalentTo(new PreferenceChangeType(true, true, true));
    }

    [Test]
    public void Should_ReturnFalse_When_PropertiesUnchanged()
    {
        // Arrange
        // Act
        PreferenceChangeType result = TestCandidate.Update(_defaultGeneralPreference, _defaultFinancePreference,
            _defaultNotificationPreference);

        // Assert
        result.Should().BeEquivalentTo(new PreferenceChangeType(false, false, false));
    }

    [Test]
    public void Should_SetCorrectValues_When_PropertiesChanged()
    {
        // Arrange
        GeneralPreference generalPreference = GeneralPreference.Create(true);
        NotificationPreference notificationPreference = NotificationPreference.Create(false, 0);
        FinancePreference financePreference = FinancePreference.Create(true, 2, true, 5);

        // Act
        TestCandidate.Update(generalPreference, financePreference, notificationPreference);

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
        TestCandidate.Update(_defaultGeneralPreference, _defaultFinancePreference, _defaultNotificationPreference);

        // Assert
        TestCandidate.GeneralPreference.Should().Be(_defaultGeneralPreference);
        TestCandidate.FinancePreference.Should().Be(_defaultFinancePreference);
        TestCandidate.NotificationPreference.Should().Be(_defaultNotificationPreference);
    }
}