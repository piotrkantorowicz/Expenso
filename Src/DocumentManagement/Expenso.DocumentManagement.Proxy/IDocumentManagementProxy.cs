using Expenso.DocumentManagement.Proxy.DTO.API.DeleteFiles.Request;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Response;
using Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;

namespace Expenso.DocumentManagement.Proxy;

public interface IDocumentManagementProxy
{
    Task<IEnumerable<GetFilesResponse>?> GetFilesAsync(Guid? userId, string[]? groups, string[] fileNames,
        GetFilesRequest_FileType fileType, CancellationToken cancellationToken = default);

    Task UploadFilesAsync(Guid? userId, string[]? groups, UploadFilesRequest_File[] files,
        UploadFilesRequest_FileType fileType, CancellationToken cancellationToken = default);

    Task DeleteFilesAsync(Guid? userId, string[]? groups, string[] fileNames, DeleteFilesRequest_FileType fileType,
        CancellationToken cancellationToken = default);
}