using Expenso.Api.Configuration.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.CorsSettingsValidator;

using TestCandidate = Api.Configuration.Settings.Services.Validators.CorsSettingsValidator;

internal abstract class CorsSettingsValidatorTestBase : TestBase<TestCandidate>
{
    protected CorsSettings _corsSettings = null!;

    [SetUp]
    public void SetUp()
    {
        _corsSettings = new CorsSettings
        {
            Enabled = true,
            AllowedOrigins = ["https://localhost:3000"]
        };

        TestCandidate = new TestCandidate();
    }
}