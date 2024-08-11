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
        commandDispatcher ?? throw new ArgumentNullException(paramName: nameof(commandDispatcher));

    private readonly IMessageContextFactory _messageContextFactory = messageContextFactory ??
                                                                     throw new ArgumentNullException(
                                                                         paramName: nameof(messageContextFactory));

    private readonly IQueryDispatcher _queryDispatcher =
        queryDispatcher ?? throw new ArgumentNullException(paramName: nameof(queryDispatcher));

    public async Task<IEnumerable<GetFilesResponse>?> GetFilesAsync(GetFileRequest getFileRequest,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            query: new GetFilesQuery(MessageContext: _messageContextFactory.Current(), GetFileRequest: getFileRequest),
            cancellationToken: cancellationToken);
    }

    public async Task UploadFilesAsync(UploadFilesRequest uploadFilesRequest,
        CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(
            command: new UploadFilesCommand(MessageContext: _messageContextFactory.Current(),
                UploadFilesRequest: uploadFilesRequest), cancellationToken: cancellationToken);
    }

    public async Task DeleteFilesAsync(DeleteFilesRequest deleteFilesRequest,
        CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(
            command: new DeleteFilesCommand(MessageContext: _messageContextFactory.Current(),
                DeleteFilesRequest: deleteFilesRequest), cancellationToken: cancellationToken);
    }
}