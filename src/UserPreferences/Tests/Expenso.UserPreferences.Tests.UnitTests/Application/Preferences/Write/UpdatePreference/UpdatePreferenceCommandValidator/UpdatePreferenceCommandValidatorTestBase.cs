using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.UpdatePreferenceCommandValidator;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.UpdatePreference.
    UpdatePreferenceCommandValidator;

internal abstract class UpdatePreferenceCommandValidatorTestBase : TestBase<TestCandidate>
{
    protected UpdatePreferenceCommand _updatePreferenceCommand = null!;

    [SetUp]
    public void SetUp()
    {
        _updatePreferenceCommand = new UpdatePreferenceCommand(
            MessageContext: MessageContextFactoryMock.Object.Current(), PreferenceOrUserId: Guid.NewGuid(),
            Preference: new UpdatePreferenceRequest(FinancePreference: new UpdatePreferenceRequestFinancePreference(
                    AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 2, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 5),
                NotificationPreference: new UpdatePreferenceRequestNotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 3),
                GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: false)));

        TestCandidate = new TestCandidate();
    }
}