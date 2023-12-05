using Expenso.Shared.ModuleDefinition;

namespace Expenso.Shared.Tests.UnitTests.ModuleDefinition.Extensions.EndpointRegistrationExtensions;

internal abstract class EndpointRegistrationExtensionsTestBase : TestBase
{
    private EndpointRegistration _endpointRegistration = null!;

    protected EndpointRegistration TestCandidate { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        _endpointRegistration = AutoFixtureProxy.Create<EndpointRegistration>();
    }

    protected void CustomizeEndpointRegistration(string pattern)
    {
        TestCandidate = _endpointRegistration with
        {
            Pattern = pattern
        };
    }
}