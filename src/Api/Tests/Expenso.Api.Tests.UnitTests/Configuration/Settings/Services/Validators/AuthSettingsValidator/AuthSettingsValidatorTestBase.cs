using Expenso.Shared.System.Configuration.Settings.Auth;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.AuthSettingsValidator;

[TestFixture]
internal abstract class
    AuthSettingsValidatorTestBase : TestBase<Api.Configuration.Settings.Services.Validators.AuthSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _authSettings = new AuthSettings
        {
            AuthServer = AuthServer.Keycloak
        };

        TestCandidate = new Api.Configuration.Settings.Services.Validators.AuthSettingsValidator();
    }

    protected AuthSettings _authSettings = null!;
}