using Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Request;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference.CreatePreferenceCommandValidator;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Commands.CreatePreference.
    CreatePreferenceCommandValidator;

internal abstract class CreatePreferenceCommandValidatorTestBase : TestBase<TestCandidate>
{
    protected CreatePreferenceCommand _createPreferenceCommand = null!;

    [SetUp]
    public void SetUp()
    {
        _createPreferenceCommand = new CreatePreferenceCommand(new CreatePreferenceRequest(Guid.NewGuid()));
        TestCandidate = new TestCandidate();
    }
}
