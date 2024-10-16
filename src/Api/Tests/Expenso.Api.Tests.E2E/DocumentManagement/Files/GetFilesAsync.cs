using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Response;

namespace Expenso.Api.Tests.E2E.DocumentManagement.Files;

internal sealed class GetFilesAsync : DocumentManagementTestBase
{
    [Test]
    public async Task Should_ReturnsFiles()
    {
        // Arrange
        // Act
        IEnumerable<GetFilesResponse>? response = (await _documentManagementProxy.GetFilesAsync(
            getFileRequest: new GetFileRequest(UserId: UserDataInitializer.UserIds[index: 4], Groups: null,
                FileNames: ["Import-1", "Import-2"], FileType: GetFilesRequest_FileType.Import)))?.ToList();

        // Assert
        response?.Should().NotBeNull();
        response?.Should().HaveCount(expected: 2);
    }
}