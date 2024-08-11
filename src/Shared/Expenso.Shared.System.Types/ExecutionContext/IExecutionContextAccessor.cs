using Expenso.Shared.System.Types.ExecutionContext.Models;

namespace Expenso.Shared.System.Types.ExecutionContext;

public interface IExecutionContextAccessor
{
    IExecutionContext? Get();
}