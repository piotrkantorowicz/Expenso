using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.DocumentManagement.Proxy.DTO.API.DeleteFiles.Request;

namespace Expenso.Api.Tests.E2E.DocumentManagement;

internal sealed class DeleteFiles : DocumentManagementTestBase
{
    [Test]
    public void Should_DeleteFiles()
    {
        // Arrange
        // Act
        Action deleteFilesAction = () => _documentManagementProxy.DeleteFiles(UserDataInitializer.UserIds[4], null,
            ["Import-2"], DeleteFilesRequest_FileType.Import);

        // Assert
        deleteFilesAction.Should().NotThrow();
    }
}