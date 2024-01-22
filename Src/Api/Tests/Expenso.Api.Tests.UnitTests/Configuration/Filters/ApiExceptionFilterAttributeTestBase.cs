using Expenso.Api.Configuration.Errors;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Expenso.Api.Tests.UnitTests.Configuration.Filters;

internal abstract class ApiExceptionFilterAttributeTestBase : TestBase<GlobalExceptionHandler>
{
    protected readonly DefaultHttpContext _httpContext = new();
    private readonly NullLoggerFactory _loggerFactory = new();

    [SetUp]
    public void SetUp()
    {
        TestCandidate = new GlobalExceptionHandler(_loggerFactory.CreateLogger<GlobalExceptionHandler>());
    }
}