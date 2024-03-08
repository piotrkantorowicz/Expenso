using Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles;
using Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles;
using Expenso.DocumentManagement.Core.Application.Files.Write.UploadFiles;
using Expenso.DocumentManagement.Proxy;
using Expenso.DocumentManagement.Proxy.DTO.API.DeleteFiles.Request;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Response;
using Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Proxy;

internal sealed class DocumentManagementProxy(
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher,
    IMessageContextFactory messageContextFactory) : IDocumentManagementProxy
{
    private readonly ICommandDispatcher _commandDispatcher =
        commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));

    private readonly IMessageContextFactory _messageContextFactory =
        messageContextFactory ?? throw new ArgumentNullException(nameof(messageContextFactory));

    private readonly IQueryDispatcher _queryDispatcher =
        queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));

    public async Task<IEnumerable<GetFilesResponse>?> GetFiles(Guid? userId, string[]? groups, string[] fileNames,
        GetFilesRequest_FileType fileType, CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            new GetFilesQuery(_messageContextFactory.Current(), userId?.ToString(), groups, fileNames, fileType),
            cancellationToken);
    }

    public async Task UploadFiles(Guid? userId, string[]? groups, UploadFilesRequest_File[] files,
        UploadFilesRequest_FileType fileType, CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(
            new UploadFilesCommand(_messageContextFactory.Current(),
                new UploadFilesRequest(userId?.ToString(), groups, files, fileType)), cancellationToken);
    }

    public async Task DeleteFiles(Guid? userId, string[]? groups, string[] fileNames,
        DeleteFilesRequest_FileType fileType, CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(
            new DeleteFilesCommand(_messageContextFactory.Current(),
                new DeleteFilesRequest(userId?.ToString(), groups, fileNames, fileType)), cancellationToken);
    }
}