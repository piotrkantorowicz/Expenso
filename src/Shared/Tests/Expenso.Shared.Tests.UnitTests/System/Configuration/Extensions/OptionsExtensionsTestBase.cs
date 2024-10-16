using Microsoft.Extensions.Configuration;

namespace Expenso.Shared.Tests.UnitTests.System.Configuration.Extensions;

[TestFixture]
internal abstract class OptionsExtensionsTestBase : TestBase<IConfiguration>
{
    [SetUp]
    public void SetUp()
    {
        TestCandidate = new ConfigurationBuilder().AddInMemoryCollection(initialData: _myConfiguration).Build();
    }

    private readonly IDictionary<string, string?> _myConfiguration = new Dictionary<string, string?>
    {
        [key: "MyOptions:Option1"] = "Option1 value",
        [key: "MyOptions:Option2"] = "500"
    };
}

internal sealed record MyOptions
{
    public string? Option1 { get; init; }

    public int Option2 { get; init; }
}