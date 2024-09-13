using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Modules;

namespace Expenso.Api.Tests.UnitTests.Configuration.Extensions.RequestPathExtensions;

internal abstract class RequestPathExtensionTestBase
{
    protected Mock<ILoggerService<ModuleIdMiddleware>> _loggerMock = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILoggerService<ModuleIdMiddleware>>();
    }

    [TearDown]
    public void Teardown()
    {
        Modules.Clear();
    }
}