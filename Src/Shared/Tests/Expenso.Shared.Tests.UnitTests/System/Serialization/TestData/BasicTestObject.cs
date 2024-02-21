namespace Expenso.Shared.Tests.UnitTests.System.Serialization.TestData;

internal sealed class BasicTestObject
{
    public Guid PrimaryId { get; set; }

    public int SecondaryId { get; set; }

    public string? Name { get; set; }

    public decimal Number { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}