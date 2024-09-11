using Expenso.Api.Configuration.Extensions;
using Expenso.Shared.System.Logging;

namespace Expenso.Api.Configuration.Execution.Middlewares;

internal sealed class ModuleIdMiddleware
{
    internal const string ModuleMiddlewareHeaderKey = "Module";
    private readonly RequestDelegate _next;

    public ModuleIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ILoggerService<ModuleIdMiddleware>? logger =
            context.RequestServices.GetService<ILoggerService<ModuleIdMiddleware>>();

        string? requestPath = context.Request.Path;
        string? moduleId = requestPath.GuessModule(logger: logger);
        context.Request.Headers.Append(key: ModuleMiddlewareHeaderKey, value: moduleId);

        context.Response.OnStarting(callback: () =>
        {
            context.Response.Headers[key: ModuleMiddlewareHeaderKey] = moduleId;

            return Task.CompletedTask;
        });

        await _next.Invoke(context: context);
    }
}