namespace Expenso.Shared.System.Types.ExecutionContext.Models;

public interface IExecutionContext
{
    string? ModuleId { get; }

    Guid? CorrelationId { get; }

    IUserContext? UserContext { get; }
}