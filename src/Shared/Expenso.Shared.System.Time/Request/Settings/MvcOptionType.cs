namespace Expenso.Shared.System.Time.Request.Settings;

[Flags]
public enum MvcOptionType
{
    None = 0,
    MinimalApi = 1,
    Controllers = 2,
    All = MinimalApi | Controllers
}