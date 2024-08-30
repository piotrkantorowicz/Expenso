using Expenso.Shared.System.Modules;

namespace Expenso.Api.Configuration.Execution.Middlewares;

internal sealed class ModuleIdMiddleware
{
    internal const string ModuleMiddlewareHeaderKey = "Module";
    private readonly RequestDelegate _next;

    public ModuleIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string? moduleId = Modules.GuessModule(requestPath: context.Request.Path);
        context.Request.Headers.Append(key: ModuleMiddlewareHeaderKey, value: moduleId);
        await _next.Invoke(context: context);
    }
}