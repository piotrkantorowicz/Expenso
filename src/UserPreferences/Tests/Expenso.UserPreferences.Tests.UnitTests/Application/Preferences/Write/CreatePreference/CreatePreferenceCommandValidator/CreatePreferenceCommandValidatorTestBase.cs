using Expenso.Shared.Commands.Validation;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Request.Validators;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;

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
            Payload: new CreatePreferenceRequest(UserId: Guid.NewGuid()));

        TestCandidate = new TestCandidate(messageContextValidator: new MessageContextValidator(),
            preferenceRequestValidator: new CreatePreferenceRequestValidator());
    }
}