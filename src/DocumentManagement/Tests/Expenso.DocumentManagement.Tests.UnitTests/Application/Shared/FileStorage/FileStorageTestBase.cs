using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.FileStorage;

[TestFixture]
internal abstract class FileStorageTestBase : TestBase<IFileStorage>
{
    [SetUp]
    public void Setup()
    {
        _fileSystemMock = new Mock<IFileSystem>();
        TestCandidate = new Core.Application.Shared.Services.Acl.Disk.FileStorage(fileSystem: _fileSystemMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _fileSystemMock.Reset();
        _fileSystemMock = null!;
        TestCandidate = null!;
    }

    protected Mock<IFileSystem> _fileSystemMock = null!;
}