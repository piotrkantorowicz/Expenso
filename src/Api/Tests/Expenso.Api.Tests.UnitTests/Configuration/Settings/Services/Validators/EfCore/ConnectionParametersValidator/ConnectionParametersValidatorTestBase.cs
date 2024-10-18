using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.EfCore.ConnectionParametersValidator;

[TestFixture]
internal abstract class
    ConnectionParametersValidatorTestBase : TestBase<
    Api.Configuration.Settings.Services.Validators.EfCore.ConnectionParametersValidator>
{
    [SetUp]
    public void SetUp()
    {
        _connectionParameters = new ConnectionParameters
        {
            Host = "valid-host.com",
            Port = "5432",
            DefaultDatabase = "ValidDefaultDB",
            Database = "ValidDB",
            User = "ValidUser",
            Password = "ValidPassword1!"
        };

        TestCandidate = new Api.Configuration.Settings.Services.Validators.EfCore.ConnectionParametersValidator();
    }

    protected ConnectionParameters _connectionParameters = null!;
}