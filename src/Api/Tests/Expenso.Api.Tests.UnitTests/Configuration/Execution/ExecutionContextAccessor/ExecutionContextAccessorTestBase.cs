using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

using Moq;

using NUnit.Framework;

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

    [TearDown]
    public void TearDown()
    {
        _httpContextAccessorMock.Reset();
        _httpContextAccessorMock = null!;
        TestCandidate = null!;
    }

    protected Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;
}