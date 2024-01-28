using Microsoft.Extensions.Configuration;

namespace Expenso.Shared.Tests.UnitTests.Configuration.Extensions;

internal abstract class OptionsExtensionsTestBase : TestBase<IConfiguration>
{
    private readonly IDictionary<string, string?> _myConfiguration = new Dictionary<string, string?>
    {
        ["MyOptions:Option1"] = "Option1 value",
        ["MyOptions:Option2"] = "500"
    };

    [SetUp]
    public void SetUp()
    {
        TestCandidate = new ConfigurationBuilder().AddInMemoryCollection(_myConfiguration).Build();
    }
}

internal sealed record MyOptions
{
    public string? Option1 { get; init; }

    public int Option2 { get; init; }
}