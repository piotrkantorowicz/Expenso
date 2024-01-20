using Expenso.UserPreferences.Core.Application.Preferences.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Validators.UpdatePreference;

internal abstract class UpdatePreferenceCommandValidatorTestBase : TestBase<UpdatePreferenceCommandValidator>
{
    protected UpdatePreferenceCommand _updatePreferenceCommand = null!;

    [SetUp]
    public void SetUp()
    {
        _updatePreferenceCommand = new UpdatePreferenceCommand(Guid.NewGuid(),
            new UpdatePreferenceRequest(new UpdateFinancePreferenceRequest(true, 2, true, 5),
                new UpdateNotificationPreferenceRequest(true, 3), new UpdateGeneralPreferenceRequest(false)));

        TestCandidate = new UpdatePreferenceCommandValidator();
    }
}