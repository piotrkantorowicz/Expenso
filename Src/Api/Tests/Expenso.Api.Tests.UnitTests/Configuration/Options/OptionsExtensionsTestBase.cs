using Expenso.Shared.Tests.UnitTests.Utils;
using Microsoft.Extensions.Configuration;

namespace Expenso.Api.Tests.UnitTests.Configuration.Options;

internal abstract class OptionsExtensionsTestBase : TestBase
{
    private readonly IDictionary<string, string?> _myConfiguration = new Dictionary<string, string?>
    {
        ["MyOptions:Option1"] = "Option1 value", ["MyOptions:Option2"] = "500"
    };

    protected IConfiguration Configuration { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Configuration = new ConfigurationBuilder().AddInMemoryCollection(_myConfiguration).Build();
    }
}

internal sealed record MyOptions
{
    public string? Option1 { get; init; }

    public int Option2 { get; init; }
}