namespace Expenso.Shared.Tests.UnitTests.System.Serialization.TestData;

internal class RichTestObject
{
    public RichTestObject(Guid primaryId, int secondaryId, string name, decimal number, DateTimeOffset createdAt,
        IEnumerable<BasicTestObject> items)
    {
        PrimaryId = primaryId;
        SecondaryId = secondaryId;
        Name = name;
        Number = number;
        CreatedAt = createdAt;
        Items = items;
    }

    public Guid PrimaryId { get; }

    public int SecondaryId { get; init; }

    public string Name { get; private set; }

    public decimal Number { get; protected set; }

    public DateTimeOffset CreatedAt { get; protected internal set; }

    public IEnumerable<BasicTestObject> Items { get; }
}