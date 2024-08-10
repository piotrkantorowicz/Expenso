using Expenso.Shared.System.Modules;

namespace Expenso.Shared.Tests.UnitTests.System.Modules.Extensions.EndpointRegistrationExtensions;

internal abstract class EndpointRegistrationExtensionsTestBase : TestBase<EndpointRegistration>
{
    private EndpointRegistration _endpointRegistration = null!;

    [SetUp]
    public void SetUp()
    {
        _endpointRegistration = new EndpointRegistration(Pattern: "/pattern", Name: "GET",
            AccessControl: AccessControl.Anonymous, HttpVerb: HttpVerb.Get, Handler: null);
    }

    protected void CustomizeEndpointRegistration(string pattern)
    {
        TestCandidate = _endpointRegistration with
        {
            Pattern = pattern
        };
    }
}