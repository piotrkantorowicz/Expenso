using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.Validators.EfCore.EfCoreSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Validators.EfCore.EfCoreValidator;

internal abstract class EfCoreSettingsValidatorTestBase : TestBase<TestCandidate>
{
    protected EfCoreSettings _efCoreSettings = null!;

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

        TestCandidate = new TestCandidate();
    }
}