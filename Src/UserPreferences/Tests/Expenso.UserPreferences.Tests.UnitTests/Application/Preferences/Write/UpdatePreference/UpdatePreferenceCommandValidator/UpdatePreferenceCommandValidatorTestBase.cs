using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.
    UpdatePreferenceCommandValidator;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.UpdatePreference.
    UpdatePreferenceCommandValidator;

internal abstract class UpdatePreferenceCommandValidatorTestBase : TestBase<TestCandidate>
{
    protected UpdatePreferenceCommand _updatePreferenceCommand = null!;

    [SetUp]
    public void SetUp()
    {
        _updatePreferenceCommand = new UpdatePreferenceCommand(MessageContextFactoryMock.Object.Current(),
            Guid.NewGuid(),
            new UpdatePreferenceRequest(new UpdatePreferenceRequest_FinancePreference(true, 2, true, 5),
                new UpdatePreferenceRequest_NotificationPreference(true, 3),
                new UpdatePreferenceRequest_GeneralPreference(false)));

        TestCandidate = new TestCandidate();
    }
}