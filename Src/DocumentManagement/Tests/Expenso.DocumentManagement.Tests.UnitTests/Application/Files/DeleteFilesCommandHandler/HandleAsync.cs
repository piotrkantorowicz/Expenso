using Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles;
using Expenso.DocumentManagement.Proxy.DTO.API.DeleteFiles.Request;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Files.DeleteFilesCommandHandler;

internal sealed class HandleAsync : DeleteFilesCommandHandlerTestBase
{
    [Test]
    public async Task Should_DeleteFiles()
    {
        // Arrange
        const string directoryPath = "directoryPath";

        string[] fileNames =
        [
            "fileName1",
            "fileName2"
        ];

        string userId = Guid.NewGuid().ToString();

        DeleteFilesCommand command = new(MessageContextFactoryMock.Object.Current(),
            new DeleteFilesRequest(userId, null, fileNames, DeleteFilesRequest_FileType.Import));

        _fileStorageMock
            .Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _directoryPathResolverMock
            .Setup(x => x.ResolvePath((int)DeleteFilesRequest_FileType.Import, userId,
                command.DeleteFilesRequest.Groups))
            .Returns(directoryPath);

        _fileSystemMock.Setup(x => x.Path.Combine(directoryPath, fileNames[0])).Returns("filePath1");
        _fileSystemMock.Setup(x => x.Path.Combine(directoryPath, fileNames[1])).Returns("filePath2");

        // Act
        await TestCandidate.HandleAsync(command, default);

        // Assert
        _fileStorageMock.Verify(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }
}