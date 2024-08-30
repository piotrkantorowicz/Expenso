using Expenso.Shared.System.Types.ExecutionContext.Models;

namespace Expenso.Api.Configuration.Execution;

internal sealed record ExecutionContext(string? ModuleId, Guid? CorrelationId, IUserContext? UserContext)
    : IExecutionContext;