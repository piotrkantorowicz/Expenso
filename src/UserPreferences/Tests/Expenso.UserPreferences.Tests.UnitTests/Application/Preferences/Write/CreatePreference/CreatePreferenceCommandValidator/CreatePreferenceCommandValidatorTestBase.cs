using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Requests;

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
        _createPreferenceCommand = new CreatePreferenceCommand(
            MessageContext: MessageContextFactoryMock.Object.Current(),
            Preference: new CreatePreferenceRequest(UserId: Guid.NewGuid()));

        TestCandidate = new TestCandidate();
    }
}