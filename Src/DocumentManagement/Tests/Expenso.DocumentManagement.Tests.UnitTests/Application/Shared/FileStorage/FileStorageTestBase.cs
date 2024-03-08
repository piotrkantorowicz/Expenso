using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate = Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk.FileStorage;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.FileStorage;

internal abstract class FileStorageTestBase : TestBase<IFileStorage>
{
    protected Mock<IFileSystem> _fileSystemMock = null!;

    [SetUp]
    public void Setup()
    {
        _fileSystemMock = new Mock<IFileSystem>();
        TestCandidate = new TestCandidate(_fileSystemMock.Object);
    }
}