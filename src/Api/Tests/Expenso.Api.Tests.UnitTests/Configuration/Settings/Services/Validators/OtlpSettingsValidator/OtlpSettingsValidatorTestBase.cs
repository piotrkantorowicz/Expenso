using Expenso.Shared.System.Metrics;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.OtlpSettingsValidator;

[TestFixture]
internal abstract class
    OtlpSettingsValidatorTestBase : TestBase<Api.Configuration.Settings.Services.Validators.OtlpSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _otlpSettings = new OtlpSettings
        {
            ServiceName = "ValidServiceName",
            Endpoint = "https://valid-endpoint.com"
        };

        TestCandidate = new Api.Configuration.Settings.Services.Validators.OtlpSettingsValidator();
    }

    protected OtlpSettings _otlpSettings = null!;
}