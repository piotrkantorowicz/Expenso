using Expenso.Shared.Tests.Utils.UnitTests;

using Keycloak.AuthServices.Common;

using NUnit.Framework;

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
            Secret = Guid.CreateVersion7().ToString()
        };

        TestCandidate = new Api.Configuration.Settings.Services.Validators.Keycloak.CredentialsValidator();
    }

    [TearDown]
    public void TearDown()
    {
        _credentials = null!;
        TestCandidate = null!;
    }

    protected KeycloakClientInstallationCredentials _credentials = null!;
}