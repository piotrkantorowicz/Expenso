using Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles;
using Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles;
using Expenso.DocumentManagement.Core.Application.Files.Write.UploadFiles;
using Expenso.DocumentManagement.Shared;
using Expenso.DocumentManagement.Shared.DTO.API.DeleteFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Response;
using Expenso.DocumentManagement.Shared.DTO.API.UploadFiles.Request;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Proxy;

internal sealed class DocumentManagementProxy : IDocumentManagementProxy
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IMessageContextFactory _messageContextFactory;
    private readonly IQueryDispatcher _queryDispatcher;

    public DocumentManagementProxy(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IMessageContextFactory messageContextFactory)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(paramName: nameof(commandDispatcher));

        _messageContextFactory = messageContextFactory ??
                                 throw new ArgumentNullException(paramName: nameof(messageContextFactory));

        _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(paramName: nameof(queryDispatcher));
    }

    public async Task<IEnumerable<GetFilesResponse>?> GetFilesAsync(GetFileRequest getFileRequest,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            query: new GetFilesQuery(MessageContext: _messageContextFactory.Current(), Payload: getFileRequest),
            cancellationToken: cancellationToken);
    }

    public async Task UploadFilesAsync(UploadFilesRequest uploadFilesRequest,
        CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(
            command: new UploadFilesCommand(MessageContext: _messageContextFactory.Current(),
                Payload: uploadFilesRequest), cancellationToken: cancellationToken);
    }

    public async Task DeleteFilesAsync(DeleteFilesRequest deleteFilesRequest,
        CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(
            command: new DeleteFilesCommand(MessageContext: _messageContextFactory.Current(),
                Payload: deleteFilesRequest), cancellationToken: cancellationToken);
    }
}