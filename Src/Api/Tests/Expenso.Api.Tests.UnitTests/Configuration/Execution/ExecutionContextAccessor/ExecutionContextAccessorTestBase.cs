using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

using TestCandidate = Expenso.Api.Configuration.Execution.ExecutionContextAccessor;

namespace Expenso.Api.Tests.UnitTests.Configuration.Execution.ExecutionContextAccessor;

internal abstract class ExecutionContextAccessorTestBase : TestBase<IExecutionContextAccessor>
{
    protected Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        TestCandidate = new TestCandidate(_httpContextAccessorMock.Object);
    }
}