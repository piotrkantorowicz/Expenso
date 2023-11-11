using Expenso.Api.Configuration.Filters;
using Expenso.Shared.Tests.UnitTests.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace Expenso.Api.Tests.UnitTests.Configuration.Filters;

internal abstract class ApiExceptionFilterAttributeTestBase : TestBase
{
    protected ApiExceptionFilterAttribute TestCandidate { get; private set; } = null!;

    protected DefaultHttpContext HttpClient { get; private set; } = null!;

    protected ActionContext ActionContext { get; private set; } = null!;

    protected ExceptionContext ExceptionContext { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        DefaultHttpContext httpContext = new();

        ActionContext actionContext = new(httpContext, new RouteData(), new ActionDescriptor(),
            new ModelStateDictionary());

        ExceptionContext actionExecutingContext = new(actionContext, new List<IFilterMetadata>());

        HttpClient = httpContext;
        ActionContext = actionContext;
        ExceptionContext = actionExecutingContext;

        TestCandidate = new ApiExceptionFilterAttribute();
    }
}