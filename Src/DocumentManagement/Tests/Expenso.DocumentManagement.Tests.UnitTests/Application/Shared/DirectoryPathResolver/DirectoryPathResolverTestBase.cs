using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate = Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk.DirectoryPathResolver;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.DirectoryPathResolver;

internal abstract class DirectoryPathResolverTestBase : TestBase<IDirectoryPathResolver>
{
    protected Mock<IClock> _clockMock = null!;
    protected Mock<IDirectoryInfoService> _directoryInfoServiceMock = null!;

    [SetUp]
    public void Setup()
    {
        _directoryInfoServiceMock = new Mock<IDirectoryInfoService>();
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
        TestCandidate = new TestCandidate(_directoryInfoServiceMock.Object, _clockMock.Object);
    }
}