using Expenso.Shared.Commands.Validation.Validators;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request.Validators;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.UpdatePreference.
    UpdatePreferenceCommandValidator;

[TestFixture]
internal abstract class UpdatePreferenceCommandValidatorTestBase : TestBase<
    Core.Application.Preferences.Write.Commands.UpdatePreference.UpdatePreferenceCommandValidator>
{
    [SetUp]
    public void SetUp()
    {
        _updatePreferenceCommand = new UpdatePreferenceCommand(
            MessageContext: MessageContextFactoryMock.Object.Current(), PreferenceId: Guid.NewGuid(),
            Payload: new UpdatePreferenceRequest(FinancePreference: new UpdatePreferenceRequestFinancePreference(
                    AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 2, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 5),
                NotificationPreference: new UpdatePreferenceRequestNotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 3),
                GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: false)));

        TestCandidate =
            new Core.Application.Preferences.Write.Commands.UpdatePreference.UpdatePreferenceCommandValidator(
                messageContextValidator: new MessageContextValidator(),
                updatePreferenceCommandValidator: new UpdatePreferenceRequestValidator(
                    financePreferenceValidator: new UpdatePreferenceRequestFinancePreferenceValidator(),
                    notificationPreferenceValidator: new UpdatePreferenceRequestNotificationPreferenceValidator(),
                    generalPreferenceValidator: new UpdatePreferenceRequestGeneralPreferenceValidator()));
    }

    protected UpdatePreferenceCommand _updatePreferenceCommand = null!;
}