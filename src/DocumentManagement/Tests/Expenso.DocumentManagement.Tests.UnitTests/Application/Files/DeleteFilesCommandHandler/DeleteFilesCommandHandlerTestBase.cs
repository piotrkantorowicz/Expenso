using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Files.DeleteFilesCommandHandler;

[TestFixture]
internal abstract class
    DeleteFilesCommandHandlerTestBase : TestBase<Core.Application.Files.Write.DeleteFiles.DeleteFilesCommandHandler>
{
    [SetUp]
    public void Setup()
    {
        _fileStorageMock = new Mock<IFileStorage>();
        _directoryPathResolverMock = new Mock<IDirectoryPathResolver>();
        _fileSystemMock = new Mock<IFileSystem>();

        TestCandidate = new Core.Application.Files.Write.DeleteFiles.DeleteFilesCommandHandler(
            fileStorage: _fileStorageMock.Object,
            directoryPathResolver: _directoryPathResolverMock.Object, fileSystem: _fileSystemMock.Object);
    }

    protected Mock<IDirectoryPathResolver> _directoryPathResolverMock = null!;
    protected Mock<IFileStorage> _fileStorageMock = null!;
    protected Mock<IFileSystem> _fileSystemMock = null!;
}