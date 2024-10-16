using Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Response;

using FluentAssertions;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Files.GetFilesQueryHandler;

[TestFixture]
internal sealed class HandleAsync : GetFilesQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnFiles()
    {
        // Arrange
        const string directoryPath = "directoryPath";

        string[] files =
        [
            "file1",
            "file2"
        ];

        string[] filePaths =
        [
            "fileContent1",
            "fileContent2"
        ];

        byte[][] byteContents =
        [
            "q1QbZO8fTDGNShp"u8.ToArray(),
            "aFQbTdyxnb96YIn6"u8.ToArray()
        ];

        GetFilesQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            GetFileRequest: new GetFileRequest(UserId: null, Groups: null, FileNames: files,
                FileType: GetFilesRequest_FileType.Import));

        _directoryPathResolverMock
            .Setup(expression: x => x.ResolvePath((int)query.GetFileRequest.FileType,
                query.MessageContext.RequestedBy.ToString(), query.GetFileRequest.Groups))
            .Returns(value: directoryPath);

        _fileSystemMock.Setup(expression: x => x.Path.Combine(directoryPath, files[0])).Returns(value: filePaths[0]);
        _fileSystemMock.Setup(expression: x => x.Path.Combine(directoryPath, files[1])).Returns(value: filePaths[1]);

        _fileStorageMock
            .Setup(expression: x => x.ReadAsync(filePaths[0], It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: byteContents[0]);

        _fileStorageMock
            .Setup(expression: x => x.ReadAsync(filePaths[1], It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: byteContents[1]);

        // Act
        IEnumerable<GetFilesResponse>? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: default);

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectation:
            [
                new GetFilesResponse(UserId: query.MessageContext.RequestedBy, FileName: files[0],
                    FileContent: byteContents[0], FilesResponseFileType: GetFilesResponse_FileType.Import),
                new GetFilesResponse(UserId: query.MessageContext.RequestedBy, FileName: files[1],
                    FileContent: byteContents[1], FilesResponseFileType: GetFilesResponse_FileType.Import)
            ]);
    }
}