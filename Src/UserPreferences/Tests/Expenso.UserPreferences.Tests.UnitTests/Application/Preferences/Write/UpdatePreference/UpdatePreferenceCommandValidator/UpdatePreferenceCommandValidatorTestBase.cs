using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

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
        _updatePreferenceCommand = new UpdatePreferenceCommand(MessageContextFactoryMock.Object.Current(),
            Guid.NewGuid(),
            new UpdatePreferenceRequest(new UpdateFinancePreferenceRequest(true, 2, true, 5),
                new UpdateNotificationPreferenceRequest(true, 3), new UpdateGeneralPreferenceRequest(false)));

        TestCandidate = new TestCandidate();
    }
}