using Expenso.Shared.System.Metrics;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.Validators.OtlpSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.OtlpSettingsValidator;

[TestFixture]
internal abstract class OtlpSettingsValidatorTestBase : TestBase<TestCandidate>
{
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

    protected OtlpSettings _otlpSettings = null!;
}