using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.System.Configuration.Settings.Files;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.DirectoryInfoService;

[TestFixture]
internal abstract class DirectoryInfoServiceTestBase : TestBase<IDirectoryInfoService>
{
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

        TestCandidate =
            new Core.Application.Shared.Services.Acl.Disk.DirectoryInfoService(fileSystem: _fileSystemMock.Object,
                filesSettings: _filesSettings);
    }

    private FilesSettings? _filesSettings;
    protected Mock<IFileSystem>? _fileSystemMock;
}