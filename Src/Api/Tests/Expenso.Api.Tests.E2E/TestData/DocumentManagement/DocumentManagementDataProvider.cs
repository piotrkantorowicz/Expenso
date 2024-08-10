using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.DocumentManagement.Core.Application.Files.Write.UploadFiles;
using Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Api.Tests.E2E.TestData.DocumentManagement;

internal static class DocumentManagementDataInitializer
{
    private const string Addresses = "addresses";
    private const string Snakes = "snakes";
    private const string SnakesV2 = "snakes_v2";

    public static async Task Initialize(ICommandDispatcher commandDispatcher,
        IMessageContextFactory messageContextFactory, CancellationToken cancellationToken)
    {
        UploadFilesCommand command = new(MessageContext: messageContextFactory.Current(),
            UploadFilesRequest: new UploadFilesRequest(UserId: UserDataInitializer.UserIds[index: 4], Groups: null,
                Files:
                [
                    new UploadFilesRequest_File(Name: "Import-1", Content: await GetFile(fileName: Addresses)),
                    new UploadFilesRequest_File(Name: "Import-2", Content: await GetFile(fileName: Snakes)),
                    new UploadFilesRequest_File(Name: "Import-3", Content: await GetFile(fileName: SnakesV2))
                ], FileType: UploadFilesRequest_FileType.Import));

        await commandDispatcher.SendAsync(command: command, cancellationToken: cancellationToken);
    }

    private static async Task<byte[]> GetFile(string fileName)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "DocumentManagement", "Files",
            $"{fileName}.xlsx");

        return await File.ReadAllBytesAsync(path: filePath);
    }
}