using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.Tests.Utils.UnitTests;

using FileSignatures;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Files.UploadFilesCommandHandler;

using TestCandidate = Core.Application.Files.Write.UploadFiles.UploadFilesCommandHandler;

internal abstract class UploadFilesCommandHandler : TestBase<TestCandidate>
{
    protected Mock<IDirectoryPathResolver> _directoryPathResolverMock = null!;
    protected Mock<IFileFormatInspector> _fileFormatInspectorMock = null!;
    protected Mock<IFileStorage> _fileStorageMock = null!;

    [SetUp]
    public void Setup()
    {
        _fileStorageMock = new Mock<IFileStorage>();
        _directoryPathResolverMock = new Mock<IDirectoryPathResolver>();
        _fileFormatInspectorMock = new Mock<IFileFormatInspector>();

        TestCandidate = new TestCandidate(_fileStorageMock.Object, _directoryPathResolverMock.Object,
            _fileFormatInspectorMock.Object);
    }
}