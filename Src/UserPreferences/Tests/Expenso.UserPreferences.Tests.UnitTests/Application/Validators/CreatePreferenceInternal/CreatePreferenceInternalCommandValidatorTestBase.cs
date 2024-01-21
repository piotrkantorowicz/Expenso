using Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Validators.CreatePreferenceInternal;

internal abstract class
    CreatePreferenceInternalCommandValidatorTestBase : TestBase<CreatePreferenceInternalCommandValidator>
{
    protected CreatePreferenceInternalCommand _createPreferenceInternalCommand = null!;

    [SetUp]
    public void SetUp()
    {
        _createPreferenceInternalCommand =
            new CreatePreferenceInternalCommand(new CreatePreferenceInternalRequest(Guid.NewGuid()));

        TestCandidate = new CreatePreferenceInternalCommandValidator();
    }
}