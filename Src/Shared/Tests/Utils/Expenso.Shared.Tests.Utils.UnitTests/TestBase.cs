namespace Expenso.Shared.Tests.Utils.UnitTests;

public abstract class TestBase<T> where T : class
{
    protected T TestCandidate { get; set; } = null!;

    [SetUp]
    public void BaseSetUp()
    {
    }
}