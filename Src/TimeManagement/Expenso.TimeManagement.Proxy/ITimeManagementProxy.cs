using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Proxy;

public interface ITimeManagementProxy
{
    Task RegisterJobEntry(AddJobEntryRequest jobEntryRequest, CancellationToken cancellationToken = default);
}