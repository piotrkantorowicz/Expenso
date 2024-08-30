using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

using TestCandidate = Expenso.Api.Configuration.Errors.GlobalExceptionHandler;

namespace Expenso.Api.Tests.UnitTests.Configuration.Errors.GlobalExceptionHandler;

internal abstract class GlobalExceptionHandlerTestBase : TestBase<TestCandidate>
{
    protected readonly DefaultHttpContext _httpContext = new();
    private readonly Mock<ILoggerService<TestCandidate>> _loggerMock = new();

    [SetUp]
    public void SetUp()
    {
        TestCandidate = new TestCandidate(logger: _loggerMock.Object);
    }
}