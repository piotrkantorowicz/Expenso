using Microsoft.AspNetCore.Mvc.ModelBinding;

using Shouldly;

namespace Expenso.Shared.Tests.Utils.UnitTests.Assertions;

public static class ModelStateDictionaryAssertions
{
    public static void ShouldContainKey(this ModelStateDictionary modelState, string expectedKey)
    {
        modelState
            .ContainsKey(key: expectedKey)
            .ShouldBeTrue(
                customMessage: $"Expected ModelStateDictionary to contain key '{expectedKey}' but it does not.");
    }

    public static void ShouldContainValue(this ModelStateDictionary modelState, ModelStateEntry expectedValue)
    {
        modelState
            .Values.Contains(value: expectedValue)
            .ShouldBeTrue(
                customMessage: $"Expected ModelStateDictionary to contain value '{expectedValue}' but it does not.");
    }
}