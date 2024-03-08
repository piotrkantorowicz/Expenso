using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Files.GetFilesQueryHandler;

using TestCandidate = Core.Application.Files.Read.GetFiles.GetFilesQueryHandler;

internal abstract class GetFilesQueryHandlerTestBase : TestBase<TestCandidate>
{
    protected Mock<IDirectoryPathResolver> _directoryPathResolverMock = null!;
    protected Mock<IFileStorage> _fileStorageMock = null!;
    protected Mock<IFileSystem> _fileSystemMock = null!;

    [SetUp]
    public void Setup()
    {
        _fileStorageMock = new Mock<IFileStorage>();
        _directoryPathResolverMock = new Mock<IDirectoryPathResolver>();
        _fileSystemMock = new Mock<IFileSystem>();

        TestCandidate = new TestCandidate(_directoryPathResolverMock.Object, _fileStorageMock.Object,
            _fileSystemMock.Object);
    }
}