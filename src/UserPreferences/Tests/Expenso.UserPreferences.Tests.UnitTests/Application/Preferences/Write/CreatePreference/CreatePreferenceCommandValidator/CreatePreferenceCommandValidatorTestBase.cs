using Expenso.Shared.Commands.Validation.Validators;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Request.Validators;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;

using NUnit.Framework;

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
            Payload: new CreatePreferenceRequest(PreferenceId: Guid.CreateVersion7(), UserId: Guid.CreateVersion7()));

        TestCandidate =
            new Core.Application.Preferences.Write.Commands.CreatePreference.CreatePreferenceCommandValidator(
                messageContextValidator: new MessageContextValidator(),
                preferenceRequestValidator: new CreatePreferenceRequestValidator());
    }

    [TearDown]
    public void TearDown()
    {
        _createPreferenceCommand = null!;
        TestCandidate = null!;
    }

    protected CreatePreferenceCommand _createPreferenceCommand = null!;
}