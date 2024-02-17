using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.External;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.External.CreatePreferenceCommandValidator;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.External.
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