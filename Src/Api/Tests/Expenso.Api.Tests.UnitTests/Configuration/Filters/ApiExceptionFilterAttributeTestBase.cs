using Expenso.Api.Configuration.Filters;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace Expenso.Api.Tests.UnitTests.Configuration.Filters;

internal abstract class ApiExceptionFilterAttributeTestBase : TestBase<ApiExceptionFilterAttribute>
{
    protected ActionContext _actionContext = null!;
    protected ExceptionContext _exceptionContext = null!;

    [SetUp]
    public void SetUp()
    {
        DefaultHttpContext httpContext = new();

        ActionContext actionContext = new(httpContext, new RouteData(), new ActionDescriptor(),
            new ModelStateDictionary());

        ExceptionContext actionExecutingContext = new ExceptionContext(actionContext, new List<IFilterMetadata>());
        _actionContext = actionContext;
        _exceptionContext = actionExecutingContext;
        TestCandidate = new ApiExceptionFilterAttribute();
    }
}