using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

namespace Expenso.Api.Tests.UnitTests.Configuration.Execution.ExecutionContextAccessor;

[TestFixture]
internal abstract class ExecutionContextAccessorTestBase : TestBase<IExecutionContextAccessor>
{
    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        TestCandidate =
            new Api.Configuration.Execution.ExecutionContextAccessor(
                httpContextAccessor: _httpContextAccessorMock.Object);
    }

    protected Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;
}