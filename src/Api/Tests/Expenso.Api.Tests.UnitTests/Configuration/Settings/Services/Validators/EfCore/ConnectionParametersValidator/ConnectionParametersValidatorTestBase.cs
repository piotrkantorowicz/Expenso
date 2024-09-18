using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.Validators.EfCore.ConnectionParametersValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.EfCore.ConnectionParametersValidator;

internal abstract class ConnectionParametersValidatorTestBase : TestBase<TestCandidate>
{
    protected ConnectionParameters _connectionParameters = null!;

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

        TestCandidate = new TestCandidate();
    }
}