using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Response;

namespace Expenso.Api.Tests.E2E.DocumentManagement;

internal sealed class GetFilesAsync : DocumentManagementTestBase
{
    [Test]
    public async Task Should_ReturnsFiles()
    {
        // Arrange
        // Act
        IEnumerable<GetFilesResponse>? response =
            (await _documentManagementProxy.GetFilesAsync(new GetFileRequest(UserDataInitializer.UserIds[4], null,
                ["Import-1", "Import-2"], GetFilesRequest_FileType.Import)))?.ToList();

        // Assert
        response?.Should().NotBeNull();
        response?.Should().HaveCount(2);
    }
}