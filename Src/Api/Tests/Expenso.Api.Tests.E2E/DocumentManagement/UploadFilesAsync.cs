using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;

namespace Expenso.Api.Tests.E2E.DocumentManagement;

internal sealed class UploadFilesAsync : DocumentManagementTestBase
{
    [Test]
    public void Should_UploadFiles()
    {
        // Arrange
        // Act
        Action uploadFilesAction = () => _documentManagementProxy.UploadFilesAsync(new UploadFilesRequest(
            UserDataInitializer.UserIds[4], null, [
                new UploadFilesRequest_File("Import-4", [0x00, 0x01, 0x02, 0x03, 0x04])
            ], UploadFilesRequest_FileType.Import));

        // Assert
        uploadFilesAction.Should().NotThrow();
    }
}