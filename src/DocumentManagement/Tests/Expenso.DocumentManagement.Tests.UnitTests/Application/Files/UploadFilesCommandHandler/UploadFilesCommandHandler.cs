using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Files.UploadFilesCommandHandler;

using TestCandidate = Core.Application.Files.Write.UploadFiles.UploadFilesCommandHandler;

[TestFixture]
internal abstract class UploadFilesCommandHandler : TestBase<TestCandidate>
{
    [SetUp]
    public void Setup()
    {
        _fileStorageMock = new Mock<IFileStorage>();
        _directoryPathResolverMock = new Mock<IDirectoryPathResolver>();

        TestCandidate = new TestCandidate(fileStorage: _fileStorageMock.Object,
            directoryPathResolver: _directoryPathResolverMock.Object);
    }

    protected Mock<IDirectoryPathResolver> _directoryPathResolverMock = null!;
    protected Mock<IFileStorage> _fileStorageMock = null!;
}