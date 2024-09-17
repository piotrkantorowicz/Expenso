using Expenso.IAM.Core.Acl.Keycloak;
using Expenso.Shared.Tests.Utils.UnitTests;

using Keycloak.AuthServices.Common;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.Validators.KeycloakSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Validators.KeycloakSettingsValidator;

internal abstract class KeycloakSettingsValidatorTestBase : TestBase<TestCandidate>
{
    protected KeycloakSettings _keycloakSettings = null!;

    [SetUp]
    public void SetUp()
    {
        _keycloakSettings = new KeycloakSettings
        {
            AuthServerUrl = "https://auth-server-url.com",
            Realm = "ValidRealm",
            Resource = "ValidResource",
            SslRequired = "ALL",
            VerifyTokenAudience = true,
            Credentials = new KeycloakClientInstallationCredentials
            {
                Secret = Guid.NewGuid().ToString()
            }
        };

        TestCandidate = new TestCandidate();
    }
}