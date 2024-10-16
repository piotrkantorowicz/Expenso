using Expenso.Shared.System.Configuration.Settings.Auth;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.Validators.AuthSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.AuthSettingsValidator;

[TestFixture]
internal abstract class AuthSettingsValidatorTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _authSettings = new AuthSettings
        {
            AuthServer = AuthServer.Keycloak
        };

        TestCandidate = new TestCandidate();
    }

    protected AuthSettings _authSettings = null!;
}