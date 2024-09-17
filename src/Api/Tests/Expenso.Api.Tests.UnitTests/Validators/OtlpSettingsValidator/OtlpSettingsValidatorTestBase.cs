using Expenso.Shared.System.Metrics;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.Validators.OtlpSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Validators.OtlpSettingsValidator;

internal abstract class OtlpSettingsValidatorTestBase : TestBase<TestCandidate>
{
    protected OtlpSettings _otlpSettings = null!;

    [SetUp]
    public void SetUp()
    {
        _otlpSettings = new OtlpSettings
        {
            ServiceName = "ValidServiceName",
            Endpoint = "https://valid-endpoint.com"
        };

        TestCandidate = new TestCandidate();
    }
}