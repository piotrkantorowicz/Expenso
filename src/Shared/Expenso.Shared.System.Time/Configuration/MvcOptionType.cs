namespace Expenso.Shared.System.Time.Configuration;

[Flags]
public enum MvcOptionType
{
    None = 0,
    MinimalApi = 1,
    Controllers = 2,
    All = MinimalApi | Controllers
}