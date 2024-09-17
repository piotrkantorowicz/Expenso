using Expenso.Shared.System.Configuration.Settings.Auth;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.Validators.AuthSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Validators.AuthSettingsValidator;

internal abstract class AuthSettingsValidatorTestBase : TestBase<TestCandidate>
{
    protected AuthSettings _authSettings = null!;

    [SetUp]
    public void SetUp()
    {
        _authSettings = new AuthSettings
        {
            AuthServer = AuthServer.Keycloak // Assuming Keycloak is a valid enum value
        };

        TestCandidate = new TestCandidate();
    }
}