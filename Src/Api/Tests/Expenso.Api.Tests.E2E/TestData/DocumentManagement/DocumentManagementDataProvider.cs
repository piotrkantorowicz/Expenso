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

    public static async Task Initialize(ICommandDispatcher commandDispatcher,
        IMessageContextFactory messageContextFactory, CancellationToken cancellationToken)
    {
        UploadFilesCommand command = new(messageContextFactory.Current(), new UploadFilesRequest(
            UserDataInitializer.UserIds[4].ToString(), null, [
                new UploadFilesRequest_File("Import-1", await GetFile(Addresses)),
                new UploadFilesRequest_File("Import-2", await GetFile(Snakes))
            ], UploadFilesRequest_FileType.Import));

        await commandDispatcher.SendAsync(command, cancellationToken);
    }

    private static async Task<byte[]> GetFile(string fileName)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "DocumentManagement", "Files",
            $"{fileName}.xlsx");

        return await File.ReadAllBytesAsync(filePath);
    }
}