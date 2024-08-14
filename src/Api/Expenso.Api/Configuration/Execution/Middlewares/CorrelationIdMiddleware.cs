namespace Expenso.Api.Configuration.Execution.Middlewares;

internal sealed class CorrelationIdMiddleware
{
    internal const string CorrelationHeaderKey = "CorrelationId";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        Guid correlationId = Guid.NewGuid();
        context.Request.Headers.Append(key: CorrelationHeaderKey, value: correlationId.ToString());
        await _next.Invoke(context: context);
    }
}