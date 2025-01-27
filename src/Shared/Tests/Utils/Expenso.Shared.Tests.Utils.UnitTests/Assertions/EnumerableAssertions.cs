using Shouldly;

namespace Expenso.Shared.Tests.Utils.UnitTests.Assertions;

public static class EnumerableAssertions
{
    public static void ShouldContainSingle<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
    {
        collection.Count(predicate: predicate).ShouldBe(expected: 1);
    }

    public static void ShouldContainInOrder<T>(this IEnumerable<T> actual, params T[] expected)
    {
        List<T>? actualList = new(collection: actual);

        for (int i = 0; i < expected.Length; i++)
        {
            actualList[index: i].ShouldBe(expected: expected[i]);
        }
    }
}