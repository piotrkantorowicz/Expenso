using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Response;

namespace Expenso.TimeManagement.Shared;

public interface ITimeManagementProxy
{
    Task<RegisterJobEntryResponse?> RegisterJobEntry(RegisterJobEntryRequest jobEntryRequest,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default);
}