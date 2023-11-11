namespace Expenso.Shared.ModuleDefinition.Extensions;

public static class ModuleDefinitionExtensions
{
    internal static string GetModulePrefixSanitized(this ModuleDefinition moduleDefinition)
    {
        string modulePrefix = moduleDefinition.ModulePrefix;

        if (modulePrefix[0] != '/')
        {
            modulePrefix = '/' + modulePrefix;
        }

        if (modulePrefix[^1] == '/')
        {
            modulePrefix = new string(modulePrefix.AsSpan()[..^1]);
        }

        return modulePrefix;
    }
}