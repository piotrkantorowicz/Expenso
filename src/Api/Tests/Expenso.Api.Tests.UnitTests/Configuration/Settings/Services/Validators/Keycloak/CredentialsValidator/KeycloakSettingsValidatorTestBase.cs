using Expenso.Shared.Tests.Utils.UnitTests;

using Keycloak.AuthServices.Common;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Keycloak.CredentialsValidator;

[TestFixture]
internal abstract class
    KeycloakSettingsValidatorTestBase : TestBase<
    Api.Configuration.Settings.Services.Validators.Keycloak.CredentialsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _credentials = new KeycloakClientInstallationCredentials
        {
            Secret = Guid.NewGuid().ToString()
        };

        TestCandidate = new Api.Configuration.Settings.Services.Validators.Keycloak.CredentialsValidator();
    }

    protected KeycloakClientInstallationCredentials _credentials = null!;
}