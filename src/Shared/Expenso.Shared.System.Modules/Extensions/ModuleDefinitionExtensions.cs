namespace Expenso.Shared.System.Modules.Extensions;

public static class ModuleDefinitionExtensions
{
    internal static string GetModulePrefixSanitized(this IModuleDefinition moduleDefinition)
    {
        string modulePrefix = moduleDefinition.ModulePrefix;

        if (modulePrefix[index: 0] != '/')
        {
            modulePrefix = '/' + modulePrefix;
        }

        if (modulePrefix[^1] == '/')
        {
            modulePrefix = new string(value: modulePrefix.AsSpan()[..^1]);
        }

        return modulePrefix;
    }
}