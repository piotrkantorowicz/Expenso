namespace Expenso.Shared.System.Types.TypesExtensions.Validations;

public static class IntExtensions
{
    public static bool IsValidPort(this int? port)
    {
        return port is > 0 and <= 65535;
    }
}