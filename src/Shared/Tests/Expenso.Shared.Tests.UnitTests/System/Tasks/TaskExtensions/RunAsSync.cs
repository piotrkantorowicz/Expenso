using Expenso.Shared.System.Tasks;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.System.Tasks.TaskExtensions;

[TestFixture]
internal sealed class RunAsSync
{
    [Test]
    public void ShouldRunMethodSynchronously_Result()
    {
        // Arrange
        async Task<int> SumAsync(int a, int b)
        {
            await Task.Delay(millisecondsDelay: 100);

            return a * b;
        }

        // Act
        int result = SumAsync(a: 2, b: 3).RunAsSync();

        // Assert
        result.ShouldBe(expected: 6);
    }

    [Test]
    public void ShouldRunMethodSynchronously_NoResult()
    {
        // Arrange
        int number = 0;

        async Task Delay()
        {
            await Task.Delay(millisecondsDelay: 100);
            number++;
        }

        // Act
        Delay().RunAsSync();

        // Assert
        number.ShouldBe(expected: 1);
    }
}