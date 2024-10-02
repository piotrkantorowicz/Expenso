using Expenso.Shared.System.Modules;

namespace Expenso.Shared.Tests.UnitTests.System.Modules.Extensions.EndpointRegistrationExtensions;

internal abstract class EndpointRegistrationExtensionsTestBase : TestBase<EndpointRegistration>
{
    private EndpointRegistration _endpointRegistration = null!;

    [SetUp]
    public void SetUp()
    {
        _endpointRegistration = new EndpointRegistration(pattern: "/pattern", name: "GET",
            accessControl: AccessControl.Anonymous, httpVerb: HttpVerb.Get, handler: null, description: "Description",
            summary: "Summary", responses: [new Produces(StatusCode: 200)]);
    }

    protected void CustomizeEndpointRegistration(string pattern)
    {
        TestCandidate = _endpointRegistration with
        {
            Pattern = pattern
        };
    }
}