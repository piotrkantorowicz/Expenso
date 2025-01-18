using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Paging;

namespace Expenso.IAM.Shared;

public interface IIamProxy
{
    Task<GetUserByIdResponse?> GetUserByIdAsync(GetUserByIdRequest request, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default);

    Task<GetUserByEmailResponse?> GetUserByEmailAsync(GetUserByEmailRequest request,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default);

    Task<IPagedList<GetUsersResponse>?> GetUsersAsync(GetUsersRequest request, Pagination? pagination = null,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default);
}