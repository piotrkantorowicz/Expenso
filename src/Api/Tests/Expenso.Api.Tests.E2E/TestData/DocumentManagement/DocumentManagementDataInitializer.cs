using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.DocumentManagement.Shared;
using Expenso.DocumentManagement.Shared.DTO.API.UploadFiles.Request;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages;

namespace Expenso.Api.Tests.E2E.TestData.DocumentManagement;

internal static class DocumentManagementDataInitializer
{
    private const string Addresses = "addresses";
    private const string Snakes = "snakes";
    private const string SnakesV2 = "snakes_v2";

    public static async Task InitializeAsync(IDocumentManagementProxy documentManagementProxy, IClock clock,
        CancellationToken cancellationToken)
    {
        Guid correlationId = Guid.NewGuid();

        MessageContext messageContext = new(messageId: Guid.NewGuid(), correlationId: correlationId,
            requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.DocumentManagementModule);

        UploadFilesRequest uploadFilesRequest = new(UserId: UserDataInitializer.UserIds[index: 4], Groups: null, Files:
        [
            new UploadFilesRequestFile(Name: "Import-1", Content: await GetFileAsync(fileName: Addresses)),
            new UploadFilesRequestFile(Name: "Import-2", Content: await GetFileAsync(fileName: Snakes)),
            new UploadFilesRequestFile(Name: "Import-3", Content: await GetFileAsync(fileName: SnakesV2))
        ], FileType: UploadFilesRequestFileType.Import);

        await documentManagementProxy.UploadFilesAsync(uploadFilesRequest: uploadFilesRequest,
            messageContext: messageContext, cancellationToken: cancellationToken);
    }

    private static async Task<byte[]> GetFileAsync(string fileName)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "DocumentManagement", "Files",
            $"{fileName}.xlsx");

        return await File.ReadAllBytesAsync(path: filePath);
    }
}