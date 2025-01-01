using Expenso.Api.Configuration.Settings.ApiSettings;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

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

    [TearDown]
    public void TearDown()
    {
        _corsSettings = null!;
        TestCandidate = null!;
    }

    protected CorsSettings _corsSettings = null!;
}