using Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference.CreatePreferenceInternalCommandValidator;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Internal.Commands.CreatePreference.
    CreatePreferenceInternalCommandValidator;

internal abstract class CreatePreferenceInternalCommandValidatorTestBase : TestBase<TestCandidate>
{
    protected CreatePreferenceInternalCommand _createPreferenceInternalCommand = null!;

    [SetUp]
    public void SetUp()
    {
        _createPreferenceInternalCommand =
            new CreatePreferenceInternalCommand(new CreatePreferenceInternalRequest(Guid.NewGuid()));

        TestCandidate = new TestCandidate();
    }
}
