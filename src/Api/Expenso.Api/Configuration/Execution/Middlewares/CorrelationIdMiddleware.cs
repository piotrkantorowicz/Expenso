namespace Expenso.Api.Configuration.Execution.Middlewares;

internal sealed class CorrelationIdMiddleware
{
    internal const string CorrelationHeaderKey = "CorrelationId";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string correlationId = Guid.NewGuid().ToString();
        context.Request.Headers.Append(key: CorrelationHeaderKey, value: correlationId);

        context.Response.OnStarting(callback: () =>
        {
            context.Response.Headers[key: CorrelationHeaderKey] = correlationId;

            return Task.CompletedTask;
        });

        await _next.Invoke(context: context);
    }
}