using Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Validators.CreatePreference;

internal abstract class CreatePreferenceCommandValidatorTestBase : TestBase<CreatePreferenceCommandValidator>
{
    protected CreatePreferenceCommand _createPreferenceCommand = null!;

    [SetUp]
    public void SetUp()
    {
        _createPreferenceCommand = new CreatePreferenceCommand(new CreatePreferenceRequest(Guid.NewGuid()));
        TestCandidate = new CreatePreferenceCommandValidator();
    }
}