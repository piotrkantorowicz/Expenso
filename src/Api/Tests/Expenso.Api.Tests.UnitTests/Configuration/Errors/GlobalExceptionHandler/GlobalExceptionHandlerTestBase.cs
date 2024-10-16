using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

using TestCandidate = Expenso.Api.Configuration.Errors.GlobalExceptionHandler;

namespace Expenso.Api.Tests.UnitTests.Configuration.Errors.GlobalExceptionHandler;

[TestFixture]
internal abstract class GlobalExceptionHandlerTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _httpContext = new DefaultHttpContext();
        _loggerMock = new Mock<ILoggerService<TestCandidate>>();
        TestCandidate = new TestCandidate(logger: _loggerMock.Object);
    }

    protected DefaultHttpContext? _httpContext;
    private Mock<ILoggerService<TestCandidate>>? _loggerMock;

    protected static async Task<string> ReadResponse(MemoryStream memoryStream)
    {
        memoryStream.Seek(offset: 0, loc: SeekOrigin.Begin);
        using StreamReader reader = new(stream: memoryStream);

        return await reader.ReadToEndAsync();
    }
}