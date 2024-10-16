using Expenso.Shared.System.Configuration.Settings.App;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.Validators.ApplicationSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.ApplicationSettingsValidator;

[TestFixture]
internal abstract class ApplicationSettingsValidatorTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _applicationSettings = new ApplicationSettings
        {
            InstanceId = Guid.NewGuid(),
            Name = "Test Application",
            Version = "0.0.0"
        };

        TestCandidate = new TestCandidate();
    }

    protected ApplicationSettings _applicationSettings = null!;
}