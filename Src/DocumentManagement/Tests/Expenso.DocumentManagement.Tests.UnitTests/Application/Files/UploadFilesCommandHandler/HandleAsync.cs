using Expenso.DocumentManagement.Core.Application.Files.Write.UploadFiles;
using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;
using Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;

using FluentAssertions;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Files.UploadFilesCommandHandler;

internal sealed class HandleAsync : UploadFilesCommandHandler
{
    [Test]
    public async Task Should_SaveFile()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        const string fileName = "fileName";
        const string directoryPath = "directoryPath";
        byte[] byteContent = [1, 2, 3];

        UploadFilesCommand command = new(MessageContextFactoryMock.Object.Current(),
            new UploadFilesRequest(userId, null, [new UploadFilesRequest_File(fileName, byteContent)],
                UploadFilesRequest_FileType.Report));

        _directoryPathResolverMock
            .Setup(x => x.ResolvePath((int)command.UploadFilesRequest.FilesRequestFileType, userId, null))
            .Returns("directoryPath");

        _fileStorageMock
            .Setup(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte[]>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await TestCandidate.HandleAsync(command, default);

        // Assert
        _fileStorageMock.Verify(x => x.SaveAsync(directoryPath, fileName, byteContent, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public void Should_ThrowEmptyFileContentException_When_FileContentIsEmpty()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        const string fileName = "fileName";
        byte[] byteContent = Array.Empty<byte>();

        UploadFilesCommand command = new(MessageContextFactoryMock.Object.Current(),
            new UploadFilesRequest(userId, null, [new UploadFilesRequest_File(fileName, byteContent)],
                UploadFilesRequest_FileType.Report));

        _directoryPathResolverMock
            .Setup(x => x.ResolvePath((int)command.UploadFilesRequest.FilesRequestFileType, userId, null))
            .Returns("directoryPath");

        // Act
        EmptyFileContentException? exception = Assert.ThrowsAsync<EmptyFileContentException>(() =>
            TestCandidate.HandleAsync(command, default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be("One or more validation failures have occurred.");
        exception?.Details.Should().Be("File content cannot be empty.");
    }
}