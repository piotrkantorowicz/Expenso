using Expenso.TimeManagement.Proxy.DTO.RegisterJob.Requests;
using Expenso.TimeManagement.Proxy.DTO.RegisterJob.Responses;

namespace Expenso.TimeManagement.Proxy;

public interface ITimeManagementProxy
{
    Task<RegisterJobEntryResponse?> RegisterJobEntry(RegisterJobEntryRequest jobEntryRequest,
        CancellationToken cancellationToken = default);
}