using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.DocumentManagement.Proxy.DTO.API.DeleteFiles.Request;

namespace Expenso.Api.Tests.E2E.DocumentManagement.Files;

internal sealed class DeleteFilesAsync : DocumentManagementTestBase
{
    [Test]
    public void Should_DeleteFiles()
    {
        // Arrange
        // Act
        Action deleteFilesAction = () =>
            _documentManagementProxy.DeleteFilesAsync(deleteFilesRequest: new DeleteFilesRequest(
                UserId: UserDataInitializer.UserIds[index: 4], Groups: null, FileNames: ["Import-3"],
                FileType: DeleteFilesRequest_FileType.Import));

        // Assert
        deleteFilesAction.Should().NotThrow();
    }
}