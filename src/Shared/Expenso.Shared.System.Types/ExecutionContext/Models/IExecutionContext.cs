namespace Expenso.Shared.System.Types.ExecutionContext.Models;

public interface IExecutionContext
{
    Guid? CorrelationId { get; }

    IUserContext? UserContext { get; }
}