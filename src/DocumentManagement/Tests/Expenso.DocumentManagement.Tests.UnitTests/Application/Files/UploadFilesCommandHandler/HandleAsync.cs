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
        Guid userId = Guid.NewGuid();
        const string fileName = "fileName";
        const string directoryPath = "directoryPath";
        byte[] byteContent = [1, 2, 3];

        UploadFilesCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            UploadFilesRequest: new UploadFilesRequest(UserId: userId, Groups: null,
                Files: [new UploadFilesRequest_File(Name: fileName, Content: byteContent)],
                FileType: UploadFilesRequest_FileType.Report));

        _directoryPathResolverMock
            .Setup(expression: x => x.ResolvePath((int)command.UploadFilesRequest.FileType, userId.ToString(), null))
            .Returns(value: "directoryPath");

        _fileStorageMock
            .Setup(expression: x => x.SaveAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte[]>(),
                It.IsAny<CancellationToken>()))
            .Returns(value: Task.CompletedTask);

        // Act
        await TestCandidate.HandleAsync(command: command, cancellationToken: default);

        // Assert
        _fileStorageMock.Verify(
            expression: x => x.SaveAsync(directoryPath, fileName, byteContent, It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public void Should_ThrowEmptyFileContentException_When_FileContentIsEmpty()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        const string fileName = "fileName";
        byte[] byteContent = Array.Empty<byte>();

        UploadFilesCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            UploadFilesRequest: new UploadFilesRequest(UserId: userId, Groups: null,
                Files: [new UploadFilesRequest_File(Name: fileName, Content: byteContent)],
                FileType: UploadFilesRequest_FileType.Report));

        _directoryPathResolverMock
            .Setup(expression: x => x.ResolvePath((int)command.UploadFilesRequest.FileType, userId.ToString(), null))
            .Returns(value: "directoryPath");

        // Act
        EmptyFileContentException? exception = Assert.ThrowsAsync<EmptyFileContentException>(code: () =>
            TestCandidate.HandleAsync(command: command, cancellationToken: default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be(expected: "One or more validation failures have occurred.");
        exception?.Details.Should().Be(expected: "File content cannot be empty.");
    }
}