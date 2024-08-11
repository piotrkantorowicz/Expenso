using Expenso.Shared.System.Types.ExecutionContext.Models;

namespace Expenso.Api.Configuration.Execution;

internal sealed record ExecutionContext(Guid? CorrelationId, IUserContext? UserContext) : IExecutionContext;