namespace Expenso.Api.Configuration.Execution.Middlewares;

internal sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    internal const string CorrelationHeaderKey = "CorrelationId";

    public async Task Invoke(HttpContext context)
    {
        Guid correlationId = Guid.NewGuid();
        context.Request.Headers.Append(CorrelationHeaderKey, correlationId.ToString());
        await next.Invoke(context);
    }
}