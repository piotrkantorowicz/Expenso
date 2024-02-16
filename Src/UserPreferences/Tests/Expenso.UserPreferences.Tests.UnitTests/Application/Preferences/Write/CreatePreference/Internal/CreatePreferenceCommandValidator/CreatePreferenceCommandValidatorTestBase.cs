using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Request;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.CreatePreferenceCommandValidator;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.Internal.
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