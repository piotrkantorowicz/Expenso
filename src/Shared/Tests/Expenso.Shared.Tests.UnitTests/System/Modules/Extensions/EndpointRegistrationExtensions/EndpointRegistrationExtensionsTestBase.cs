using Expenso.Shared.System.Modules;

namespace Expenso.Shared.Tests.UnitTests.System.Modules.Extensions.EndpointRegistrationExtensions;

[TestFixture]
internal abstract class EndpointRegistrationExtensionsTestBase : TestBase<EndpointRegistration>
{
    [SetUp]
    public void SetUp()
    {
        _endpointRegistration = new EndpointRegistration(Pattern: "/pattern", Name: "GET",
            AccessControl: AccessControl.Anonymous, HttpVerb: HttpVerb.Get, Handler: null);
    }

    private EndpointRegistration _endpointRegistration = null!;

    protected void CustomizeEndpointRegistration(string pattern)
    {
        TestCandidate = _endpointRegistration with
        {
            Pattern = pattern
        };
    }
}