namespace Expenso.Api.Tests.E2E.Configuration;

internal sealed class TestAuth
{
    public string Token { get; private set; } = null!;

    public string TestUsername { get; private set; } = null!;
}