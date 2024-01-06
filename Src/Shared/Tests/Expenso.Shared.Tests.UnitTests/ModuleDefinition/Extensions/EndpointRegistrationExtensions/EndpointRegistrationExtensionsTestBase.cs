using Expenso.Shared.ModuleDefinition;

namespace Expenso.Shared.Tests.UnitTests.ModuleDefinition.Extensions.EndpointRegistrationExtensions;

internal abstract class EndpointRegistrationExtensionsTestBase : TestBase<EndpointRegistration>
{
    private EndpointRegistration _endpointRegistration = null!;

    [SetUp]
    public void SetUp()
    {
        _endpointRegistration =
            new EndpointRegistration("/pattern", "GET", AccessControl.Anonymous, HttpVerb.Get, null);
    }

    protected void CustomizeEndpointRegistration(string pattern)
    {
        TestCandidate = _endpointRegistration with
        {
            Pattern = pattern
        };
    }
}