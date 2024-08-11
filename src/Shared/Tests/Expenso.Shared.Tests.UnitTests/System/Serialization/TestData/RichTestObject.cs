namespace Expenso.Shared.Tests.UnitTests.System.Serialization.TestData;

internal class RichTestObject(
    Guid primaryId,
    int secondaryId,
    string name,
    decimal number,
    DateTimeOffset createdAt,
    IEnumerable<BasicTestObject> items)
{
    public Guid PrimaryId { get; } = primaryId;

    public int SecondaryId { get; init; } = secondaryId;

    public string Name { get; private set; } = name;

    public decimal Number { get; protected set; } = number;

    public DateTimeOffset CreatedAt { get; protected internal set; } = createdAt;

    public IEnumerable<BasicTestObject> Items { get; } = items;
}