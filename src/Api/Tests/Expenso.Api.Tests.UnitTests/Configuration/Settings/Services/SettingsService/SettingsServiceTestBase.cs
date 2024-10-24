using Expenso.Api.Configuration.Settings.Services;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.SettingsService;

[TestFixture]
internal abstract class SettingsServiceTestBase : TestBase<SettingsService<TestSettings>>
{
    [SetUp]
    public void SetUp()
    {
        _validatorsMock = new Mock<IEnumerable<ISettingsValidator<TestSettings>>>();
        _loggerMock = new Mock<ILoggerService<SettingsService<TestSettings>>>();
        _serviceCollection = new ServiceCollection();
        _configuration = new ConfigurationBuilder().AddInMemoryCollection(initialData: _myConfiguration).Build();

        TestCandidate = new SettingsService<TestSettings>(validators: _validatorsMock.Object,
            configuration: _configuration, logger: _loggerMock.Object, fluentValidators: []);
    }

    private readonly IDictionary<string, string?> _myConfiguration = new Dictionary<string, string?>
    {
        [key: "Test:Name"] = "Name",
        [key: "Test:IsEnabled"] = "True"
    };

    protected IConfiguration _configuration = null!;
    protected Mock<ILoggerService<SettingsService<TestSettings>>> _loggerMock = null!;
    protected IServiceCollection _serviceCollection = null!;
    protected Mock<IEnumerable<ISettingsValidator<TestSettings>>> _validatorsMock = null!;
}