using Newtonsoft.Json;

using Shouldly;

namespace Expenso.Shared.Tests.Utils.UnitTests.Assertions;

public static class DeepEqualAssertions
{
    public static void ShouldDeepEqual<T>(this T? actual, T? expected) where T : class
    {
        actual.ShouldNotBeNull();
        expected.ShouldNotBeNull();
        string? expectedJson = JsonConvert.SerializeObject(value: expected, formatting: Formatting.Indented);
        string? actualJson = JsonConvert.SerializeObject(value: actual, formatting: Formatting.Indented);
        actualJson.ShouldBe(expected: expectedJson);
    }
}