using Expenso.TimeManagement.Shared.DTO.Request;
using Expenso.TimeManagement.Shared.DTO.Response;

namespace Expenso.TimeManagement.Shared;

public interface ITimeManagementProxy
{
    Task<RegisterJobEntryResponse?> RegisterJobEntry(RegisterJobEntryRequest jobEntryRequest,
        CancellationToken cancellationToken = default);
}