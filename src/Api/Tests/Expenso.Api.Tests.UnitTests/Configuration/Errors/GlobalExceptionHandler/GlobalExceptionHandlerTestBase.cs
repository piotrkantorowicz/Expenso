using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

using TestCandidate = Expenso.Api.Configuration.Errors.GlobalExceptionHandler;

namespace Expenso.Api.Tests.UnitTests.Configuration.Errors.GlobalExceptionHandler;

internal abstract class GlobalExceptionHandlerTestBase : TestBase<TestCandidate>
{
    protected DefaultHttpContext _httpContext = new();
    private Mock<ILoggerService<TestCandidate>> _loggerMock = new();

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILoggerService<TestCandidate>>();
        _httpContext = new DefaultHttpContext();
        TestCandidate = new TestCandidate(logger: _loggerMock.Object);
    }

    protected static async Task<string> ReadResponse(MemoryStream memoryStream)
    {
        memoryStream.Seek(offset: 0, loc: SeekOrigin.Begin);
        using StreamReader reader = new(stream: memoryStream);

        return await reader.ReadToEndAsync();
    }
}