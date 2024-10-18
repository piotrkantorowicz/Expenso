using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

namespace Expenso.Api.Tests.UnitTests.Configuration.Errors.GlobalExceptionHandler;

[TestFixture]
internal abstract class GlobalExceptionHandlerTestBase : TestBase<Api.Configuration.Errors.GlobalExceptionHandler>
{
    [SetUp]
    public void SetUp()
    {
        _httpContext = new DefaultHttpContext();
        _loggerMock = new Mock<ILoggerService<Api.Configuration.Errors.GlobalExceptionHandler>>();
        TestCandidate = new Api.Configuration.Errors.GlobalExceptionHandler(logger: _loggerMock.Object);
    }

    protected DefaultHttpContext? _httpContext;
    private Mock<ILoggerService<Api.Configuration.Errors.GlobalExceptionHandler>>? _loggerMock;

    protected static async Task<string> ReadResponse(MemoryStream memoryStream)
    {
        memoryStream.Seek(offset: 0, loc: SeekOrigin.Begin);
        using StreamReader reader = new(stream: memoryStream);

        return await reader.ReadToEndAsync();
    }
}