using Expenso.Api.Configuration.Settings.Services.Validators.EfCore;
using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.EfCore.EfCoreValidator;

[TestFixture]
internal abstract class EfCoreSettingsValidatorTestBase : TestBase<EfCoreSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _efCoreSettings = new EfCoreSettings
        {
            ConnectionParameters = new ConnectionParameters
            {
                Host = "valid-host.com",
                Port = "5432",
                DefaultDatabase = "ValidDefaultDB",
                Database = "ValidDB",
                User = "ValidUser",
                Password = "ValidPassword1!"
            },
            InMemory = true,
            UseMigration = true,
            UseSeeding = true
        };

        TestCandidate = new EfCoreSettingsValidator(
            connectionParametersValidator:
            new Api.Configuration.Settings.Services.Validators.EfCore.ConnectionParametersValidator());
    }

    protected EfCoreSettings _efCoreSettings = null!;
}