using Expenso.DocumentManagement.Shared.DTO.API.DeleteFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Response;
using Expenso.DocumentManagement.Shared.DTO.API.UploadFiles.Request;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Shared;

public interface IDocumentManagementProxy
{
    Task<IEnumerable<GetFilesResponse>?> GetFilesAsync(GetFileRequest getFileRequest,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default);

    Task UploadFilesAsync(UploadFilesRequest uploadFilesRequest, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default);

    Task DeleteFilesAsync(DeleteFilesRequest deleteFilesRequest, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default);
}