using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate = Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk.FileStorage;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.FileStorage;

[TestFixture]
internal abstract class FileStorageTestBase : TestBase<IFileStorage>
{
    [SetUp]
    public void Setup()
    {
        _fileSystemMock = new Mock<IFileSystem>();
        TestCandidate = new TestCandidate(fileSystem: _fileSystemMock.Object);
    }

    protected Mock<IFileSystem> _fileSystemMock = null!;
}