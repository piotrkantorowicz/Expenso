using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.SettingsService<
    Expenso.Api.Tests.UnitTests.Configuration.Settings.TestSettings>;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.SettingsService;

[TestFixture]
internal abstract class SettingsServiceTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _validatorsMock = new Mock<IEnumerable<ISettingsValidator<TestSettings>>>();
        _loggerMock = new Mock<ILoggerService<TestCandidate>>();
        _serviceCollection = new ServiceCollection();
        _configuration = new ConfigurationBuilder().AddInMemoryCollection(initialData: _myConfiguration).Build();

        TestCandidate = new TestCandidate(validators: _validatorsMock.Object, configuration: _configuration,
            logger: _loggerMock.Object);
    }

    private readonly IDictionary<string, string?> _myConfiguration = new Dictionary<string, string?>
    {
        [key: "Test:Name"] = "Name",
        [key: "Test:IsEnabled"] = "True"
    };

    protected IConfiguration _configuration = null!;
    protected Mock<ILoggerService<TestCandidate>> _loggerMock = null!;
    protected IServiceCollection _serviceCollection = null!;
    protected Mock<IEnumerable<ISettingsValidator<TestSettings>>> _validatorsMock = null!;
}