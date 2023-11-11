using Expenso.Shared.ModuleDefinition;

namespace Expenso.Shared.Tests.UnitTests.ModuleDefinition.Extensions.EndpointRegistrationExtensions;

internal abstract class EndpointRegistrationExtensionsTestBase : TestBase
{
    protected EndpointRegistration TestCandidate { get; set; } = null!;

    protected EndpointRegistration EndpointRegistration { get; set; } = null!;

    [SetUp]
    public void SetUp()
    {
        EndpointRegistration = AutoFixtureProxy.Create<EndpointRegistration>();
    }
}