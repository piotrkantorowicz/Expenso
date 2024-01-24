using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using TestCandidate = Expenso.Api.Configuration.Errors.GlobalExceptionHandler;

namespace Expenso.Api.Tests.UnitTests.Configuration.Errors.GlobalExceptionHandler;

internal abstract class GlobalExceptionHandlerTestBase : TestBase<TestCandidate>
{
    protected readonly DefaultHttpContext _httpContext = new();
    private readonly NullLoggerFactory _loggerFactory = new();

    [SetUp]
    public void SetUp()
    {
        TestCandidate = new TestCandidate(_loggerFactory.CreateLogger<TestCandidate>());
    }
}
