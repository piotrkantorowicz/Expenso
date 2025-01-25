using Shouldly;

namespace Expenso.Shared.Tests.Utils.UnitTests.Assertions;

public static class DictionaryExtensions
{
    public static void ShouldContainKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey expectedKey)
    {
        dictionary
            .ContainsKey(key: expectedKey)
            .ShouldBeTrue(customMessage: $"Expected dictionary to contain key '{expectedKey}' but it does not.");
    }

    public static void ShouldContainValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue expectedValue)
    {
        dictionary
            .Values.Contains(item: expectedValue)
            .ShouldBeTrue(customMessage: $"Expected dictionary to contain value '{expectedValue}' but it does not.");
    }
}