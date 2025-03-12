namespace Expenso.Api.Tests.E2E.Configuration;

internal sealed class ClaimsService : IDisposable
{
    private readonly IDictionary<string, object?> _claims;

    public ClaimsService()
    {
        _claims = WebAppTestSetup.Claims.ToDictionary(keySelector: pair => pair.Key,
            elementSelector: pair => pair.Value);
    }

    public IDictionary<string, object?> GetClaims()
    {
        return _claims.ToDictionary(keySelector: pair => pair.Key, elementSelector: pair => pair.Value);
    }

    public void Replace(string key, object? value)
    {
        _claims[key: key] = value;
    }

    public void Remove(string key)
    {
        _claims.Remove(key: key);
    }

    public void Dispose()
    {
        Clear();
    }

    private void Clear()
    {
        _claims.Clear();
    }
}