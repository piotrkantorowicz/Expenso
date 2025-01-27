using Shouldly;

namespace Expenso.Shared.Tests.Utils.UnitTests.Assertions;

public static class DateTimeAssertions
{
    public static void ShouldBeCloseTo(this DateTimeOffset actual, DateTimeOffset expected, TimeSpan precision)
    {
        double difference = Math.Abs(value: (actual - expected).TotalMilliseconds);

        if (difference > precision.TotalMilliseconds)
        {
            throw new ShouldAssertException(
                message:
                $"Expected {actual} to be within {precision} of {expected} but the difference was {difference}ms");
        }
    }
}