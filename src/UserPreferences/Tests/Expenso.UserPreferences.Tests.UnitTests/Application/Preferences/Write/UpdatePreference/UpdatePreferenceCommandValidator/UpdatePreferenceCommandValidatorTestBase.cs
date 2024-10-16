using Expenso.Shared.Commands.Validation;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request.Validators;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.UpdatePreferenceCommandValidator;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.UpdatePreference.
    UpdatePreferenceCommandValidator;

[TestFixture]
internal abstract class UpdatePreferenceCommandValidatorTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _updatePreferenceCommand = new UpdatePreferenceCommand(
            MessageContext: MessageContextFactoryMock.Object.Current(), PreferenceId: Guid.NewGuid(),
            Payload: new UpdatePreferenceRequest(
                FinancePreference: new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 2, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 5),
                NotificationPreference: new UpdatePreferenceRequest_NotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 3),
                GeneralPreference: new UpdatePreferenceRequest_GeneralPreference(UseDarkMode: false)));

        TestCandidate = new TestCandidate(messageContextValidator: new MessageContextValidator(),
            updatePreferenceCommandValidator: new UpdatePreferenceRequestValidator(
                financePreferenceValidator: new UpdatePreferenceRequest_FinancePreferenceValidator(),
                notificationPreferenceValidator: new UpdatePreferenceRequest_NotificationPreferenceValidator(),
                generalPreferenceValidator: new UpdatePreferenceRequest_GeneralPreferenceValidator()));
    }

    protected UpdatePreferenceCommand _updatePreferenceCommand = null!;
}