using Expenso.DocumentManagement.Shared.DTO.API.DeleteFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Response;
using Expenso.DocumentManagement.Shared.DTO.API.UploadFiles.Request;

namespace Expenso.DocumentManagement.Shared;

public interface IDocumentManagementProxy
{
    Task<IEnumerable<GetFilesResponse>?> GetFilesAsync(GetFileRequest getFileRequest,
        CancellationToken cancellationToken = default);

    Task UploadFilesAsync(UploadFilesRequest uploadFilesRequest, CancellationToken cancellationToken = default);

    Task DeleteFilesAsync(DeleteFilesRequest deleteFilesRequest, CancellationToken cancellationToken = default);
}