using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.System.Configuration.Settings.Files;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate = Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk.DirectoryInfoService;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.DirectoryInfoService;

internal abstract class DirectoryInfoServiceTestBase : TestBase<IDirectoryInfoService>
{
    private FilesSettings? _filesSettings;
    protected Mock<IFileSystem>? _fileSystemMock;

    [SetUp]
    public void Setup()
    {
        _fileSystemMock = new Mock<IFileSystem>();

        _filesSettings = new FilesSettings
        {
            StorageType = FileStorageType.Disk,
            RootPath = "RootPath",
            ImportDirectory = "Imports",
            ReportsDirectory = "Reports"
        };

        TestCandidate = new TestCandidate(_fileSystemMock.Object, _filesSettings);
    }
}