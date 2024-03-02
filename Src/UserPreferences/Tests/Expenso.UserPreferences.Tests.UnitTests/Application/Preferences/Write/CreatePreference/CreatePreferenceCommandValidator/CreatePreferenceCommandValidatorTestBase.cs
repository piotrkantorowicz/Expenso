using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.CreatePreferenceCommandValidator;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.
    CreatePreferenceCommandValidator;

internal abstract class CreatePreferenceCommandValidatorTestBase : TestBase<TestCandidate>
{
    protected CreatePreferenceCommand _createPreferenceCommand = null!;

    [SetUp]
    public void SetUp()
    {
        _createPreferenceCommand = new CreatePreferenceCommand(MessageContextFactoryMock.Object.Current(),
            new CreatePreferenceRequest(Guid.NewGuid()));

        TestCandidate = new TestCandidate();
    }
}