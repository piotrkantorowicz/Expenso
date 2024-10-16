using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate = Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles.DeleteFilesCommandHandler;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Files.DeleteFilesCommandHandler;

[TestFixture]
internal abstract class DeleteFilesCommandHandlerTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void Setup()
    {
        _fileStorageMock = new Mock<IFileStorage>();
        _directoryPathResolverMock = new Mock<IDirectoryPathResolver>();
        _fileSystemMock = new Mock<IFileSystem>();

        TestCandidate = new TestCandidate(fileStorage: _fileStorageMock.Object,
            directoryPathResolver: _directoryPathResolverMock.Object, fileSystem: _fileSystemMock.Object);
    }

    protected Mock<IDirectoryPathResolver> _directoryPathResolverMock = null!;
    protected Mock<IFileStorage> _fileStorageMock = null!;
    protected Mock<IFileSystem> _fileSystemMock = null!;
}