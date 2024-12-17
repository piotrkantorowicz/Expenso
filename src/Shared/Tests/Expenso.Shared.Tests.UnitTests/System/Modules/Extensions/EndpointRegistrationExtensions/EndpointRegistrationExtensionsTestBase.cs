using Expenso.Shared.System.Modules;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

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

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
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