using Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles;
using Expenso.DocumentManagement.Shared.DTO.API.DeleteFiles.Request;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Files.DeleteFilesCommandHandler;

[TestFixture]
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

        Guid userId = Guid.NewGuid();

        DeleteFilesCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            DeleteFilesRequest: new DeleteFilesRequest(UserId: userId, Groups: null, FileNames: fileNames,
                FileType: DeleteFilesRequest_FileType.Import));

        _fileStorageMock
            .Setup(expression: x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(value: Task.CompletedTask);

        _directoryPathResolverMock
            .Setup(expression: x => x.ResolvePath((int)DeleteFilesRequest_FileType.Import, userId.ToString(),
                command.DeleteFilesRequest.Groups))
            .Returns(value: directoryPath);

        _fileSystemMock.Setup(expression: x => x.Path.Combine(directoryPath, fileNames[0])).Returns(value: "filePath1");
        _fileSystemMock.Setup(expression: x => x.Path.Combine(directoryPath, fileNames[1])).Returns(value: "filePath2");

        // Act
        await TestCandidate.HandleAsync(command: command, cancellationToken: default);

        // Assert
        _fileStorageMock.Verify(expression: x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            times: Times.Exactly(callCount: 2));
    }
}