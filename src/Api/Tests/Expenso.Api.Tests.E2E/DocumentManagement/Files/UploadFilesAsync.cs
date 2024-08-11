using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;

namespace Expenso.Api.Tests.E2E.DocumentManagement.Files;

internal sealed class UploadFilesAsync : DocumentManagementTestBase
{
    [Test]
    public void Should_UploadFiles()
    {
        // Arrange
        // Act
        Action uploadFilesAction = () => _documentManagementProxy.UploadFilesAsync(
            uploadFilesRequest: new UploadFilesRequest(UserId: UserDataInitializer.UserIds[index: 4], Groups: null,
                Files:
                [
                    new UploadFilesRequest_File(Name: "Import-4", Content: [0x00, 0x01, 0x02, 0x03, 0x04])
                ], FileType: UploadFilesRequest_FileType.Import));

        // Assert
        uploadFilesAction.Should().NotThrow();
    }
}