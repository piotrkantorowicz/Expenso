namespace Expenso.Shared.System.Time.Providers.Inputs;

public interface IProviderInput
{
    string Key { get; }

    string Prefix { get; }

    IProviderInput GetInput(RequestProviderType requestProviderType);
}