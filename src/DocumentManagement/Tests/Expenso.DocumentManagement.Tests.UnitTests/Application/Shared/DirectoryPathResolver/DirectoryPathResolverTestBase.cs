using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using NUnit.Framework;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.DirectoryPathResolver;

[TestFixture]
internal abstract class DirectoryPathResolverTestBase : TestBase<IDirectoryPathResolver>
{
    [SetUp]
    public void Setup()
    {
        _directoryInfoServiceMock = new Mock<IDirectoryInfoService>();
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTime.UtcNow);

        TestCandidate = new Core.Application.Shared.Services.Acl.Disk.DirectoryPathResolver(
            directoryInfoService: _directoryInfoServiceMock.Object, clock: _clockMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _directoryInfoServiceMock.Reset();
        _clockMock.Reset();
        _directoryInfoServiceMock = null!;
        _clockMock = null!;
        TestCandidate = null!;
    }

    protected Mock<IClock> _clockMock = null!;
    protected Mock<IDirectoryInfoService> _directoryInfoServiceMock = null!;
}