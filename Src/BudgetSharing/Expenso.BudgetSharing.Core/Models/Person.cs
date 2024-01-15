namespace Expenso.BudgetSharing.Core.Models;

internal sealed record Person
{
    private Person()
    {
    }

    private Person(Guid personId, string? username)
    {
        if (personId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(personId), "PersonId cannot be empty.");
        }

        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentNullException(nameof(username), "Username cannot be empty.");
        }

        PersonId = personId;
        Username = username;
    }

    public Guid PersonId { get; }

    public string? Username { get; }

    public static Person Create(string username)
    {
        return new Person(Guid.NewGuid(), username);
    }

    public static Person Create(Guid personId, string? username)
    {
        return new Person(personId, username);
    }
}