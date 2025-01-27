using Expenso.IAM.Core.Acl.Keycloak.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

using FluentValidation;

using Keycloak.AuthServices.Common;

using Moq;

using NUnit.Framework;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Keycloak.KeycloakSettingsValidator;

[TestFixture]
internal abstract class
    KeycloakSettingsValidatorTestBase : TestBase<
    Api.Configuration.Settings.Services.Validators.Keycloak.KeycloakSettingsValidator>
{
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
                Secret = Guid.CreateVersion7().ToString()
            }
        };

        Mock<IValidator<KeycloakClientInstallationCredentials>> credentialsValidatorMock = new();

        TestCandidate =
            new Api.Configuration.Settings.Services.Validators.Keycloak.KeycloakSettingsValidator(
                credentialsValidator: credentialsValidatorMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _keycloakSettings = null!;
        TestCandidate = null!;
    }

    protected KeycloakSettings _keycloakSettings = null!;
}