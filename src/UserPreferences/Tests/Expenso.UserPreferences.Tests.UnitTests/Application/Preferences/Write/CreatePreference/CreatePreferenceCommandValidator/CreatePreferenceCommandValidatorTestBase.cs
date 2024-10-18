using Expenso.Shared.Commands.Validation.Validators;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Request.Validators;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.
    CreatePreferenceCommandValidator;

[TestFixture]
internal abstract class CreatePreferenceCommandValidatorTestBase : TestBase<
    Core.Application.Preferences.Write.Commands.CreatePreference.CreatePreferenceCommandValidator>
{
    [SetUp]
    public void SetUp()
    {
        _createPreferenceCommand = new CreatePreferenceCommand(
            MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new CreatePreferenceRequest(UserId: Guid.NewGuid()));

        TestCandidate =
            new Core.Application.Preferences.Write.Commands.CreatePreference.CreatePreferenceCommandValidator(
                messageContextValidator: new MessageContextValidator(),
            preferenceRequestValidator: new CreatePreferenceRequestValidator());
    }

    protected CreatePreferenceCommand _createPreferenceCommand = null!;
}