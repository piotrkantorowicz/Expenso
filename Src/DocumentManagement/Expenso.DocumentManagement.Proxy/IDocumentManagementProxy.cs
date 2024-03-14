using Expenso.DocumentManagement.Proxy.DTO.API.DeleteFiles.Request;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Response;
using Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;

namespace Expenso.DocumentManagement.Proxy;

public interface IDocumentManagementProxy
{
    Task<IEnumerable<GetFilesResponse>?> GetFilesAsync(GetFileRequest getFileRequest,
        CancellationToken cancellationToken = default);

    Task UploadFilesAsync(UploadFilesRequest uploadFilesRequest, CancellationToken cancellationToken = default);

    Task DeleteFilesAsync(DeleteFilesRequest deleteFilesRequest, CancellationToken cancellationToken = default);
}