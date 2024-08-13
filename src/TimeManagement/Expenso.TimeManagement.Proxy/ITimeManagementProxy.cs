using Expenso.TimeManagement.Proxy.DTO.Request;
using Expenso.TimeManagement.Proxy.DTO.Response;

namespace Expenso.TimeManagement.Proxy;

public interface ITimeManagementProxy
{
    Task<RegisterJobEntryResponse?> RegisterJobEntry(RegisterJobEntryRequest jobEntryRequest, CancellationToken cancellationToken = default);
}