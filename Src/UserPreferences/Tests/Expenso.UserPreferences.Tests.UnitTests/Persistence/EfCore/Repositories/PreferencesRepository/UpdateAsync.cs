using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

internal sealed class UpdateAsync : PreferenceRepositoryTestBase
{
    [Test]
    public async Task Should_UpdatePreference_When_PreferenceExists()
    {
        // Arrange
        Preference dbPreference = Preferences.First();

        dbPreference.FinancePreference = new FinancePreference
        {
            AllowAddFinancePlanSubOwners = true,
            MaxNumberOfSubFinancePlanSubOwners = 5,
            AllowAddFinancePlanReviewers = true,
            MaxNumberOfFinancePlanReviewers = 8
        };

        // Act
        Preference updatedPreference =
            await TestCandidate.UpdateAsync(preference: dbPreference, cancellationToken: default);

        // Assert
        updatedPreference.Should().NotBeNull();
        _preferenceDbSetMock.Object.Should().Contain(expected: updatedPreference);
    }
}