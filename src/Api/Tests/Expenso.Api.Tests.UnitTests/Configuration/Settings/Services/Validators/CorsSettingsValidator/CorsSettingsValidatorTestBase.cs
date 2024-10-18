using Expenso.Api.Configuration.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.CorsSettingsValidator;

[TestFixture]
internal abstract class
    CorsSettingsValidatorTestBase : TestBase<Api.Configuration.Settings.Services.Validators.CorsSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _corsSettings = new CorsSettings
        {
            Enabled = true,
            AllowedOrigins = ["https://localhost:3000"]
        };

        TestCandidate = new Api.Configuration.Settings.Services.Validators.CorsSettingsValidator();
    }

    protected CorsSettings _corsSettings = null!;
}