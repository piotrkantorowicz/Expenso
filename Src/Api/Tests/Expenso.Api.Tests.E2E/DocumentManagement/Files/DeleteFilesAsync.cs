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
            _documentManagementProxy.DeleteFilesAsync(new DeleteFilesRequest(UserDataInitializer.UserIds[4], null,
                ["Import-3"], DeleteFilesRequest_FileType.Import));

        // Assert
        deleteFilesAction.Should().NotThrow();
    }
}