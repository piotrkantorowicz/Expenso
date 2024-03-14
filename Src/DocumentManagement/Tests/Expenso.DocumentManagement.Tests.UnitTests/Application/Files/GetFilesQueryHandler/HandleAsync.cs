using Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Response;

using FluentAssertions;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Files.GetFilesQueryHandler;

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

        GetFilesQuery query = new(MessageContextFactoryMock.Object.Current(),
            new GetFileRequest(null, null, files, GetFilesRequest_FileType.Import));

        _directoryPathResolverMock
            .Setup(x => x.ResolvePath((int)query.GetFileRequest.FileType, query.MessageContext.RequestedBy.ToString(),
                query.GetFileRequest.Groups))
            .Returns(directoryPath);

        _fileSystemMock.Setup(x => x.Path.Combine(directoryPath, files[0])).Returns(filePaths[0]);
        _fileSystemMock.Setup(x => x.Path.Combine(directoryPath, files[1])).Returns(filePaths[1]);

        _fileStorageMock
            .Setup(x => x.ReadAsync(filePaths[0], It.IsAny<CancellationToken>()))
            .ReturnsAsync(byteContents[0]);

        _fileStorageMock
            .Setup(x => x.ReadAsync(filePaths[1], It.IsAny<CancellationToken>()))
            .ReturnsAsync(byteContents[1]);

        // Act
        IEnumerable<GetFilesResponse>? result = await TestCandidate.HandleAsync(query, default);

        // Assert
        result
            .Should()
            .BeEquivalentTo([
                new GetFilesResponse(query.MessageContext.RequestedBy, files[0], byteContents[0],
                    GetFilesResponse_FileType.Import),
                new GetFilesResponse(query.MessageContext.RequestedBy, files[1], byteContents[1],
                    GetFilesResponse_FileType.Import)
            ]);
    }
}