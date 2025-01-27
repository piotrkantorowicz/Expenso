using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using NUnit.Framework;

using Shouldly;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

[TestFixture]
internal sealed class UpdateAsync : PreferenceRepositoryTestBase
{
    [Test]
    public async Task Should_UpdatePreference_When_PreferenceExists()
    {
        // Arrange
        Preference dbPreference = Preferences[index: 0];

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
        updatedPreference.ShouldNotBeNull();
        _preferenceDbSetMock.Object.ShouldContain(expected: updatedPreference);
    }
}