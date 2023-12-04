namespace Expenso.Shared.Tests.Utils.UnitTests;

public abstract class TestBase
{
    protected Fixture AutoFixtureProxy { get; private set; } = null!;

    [SetUp]
    public void BaseSetUp()
    {
        AutoFixtureProxy = new Fixture();
        AutoFixtureProxy.Customize(new AutoMoqCustomization { ConfigureMembers = true });

        AutoFixtureProxy
            .Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => AutoFixtureProxy.Behaviors.Remove(b));

        AutoFixtureProxy.Behaviors.Add(new OmitOnRecursionBehavior(10));
    }
}