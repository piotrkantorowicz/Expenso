using Expenso.Shared.System.Configuration.Settings.Files;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class FilesSettingsValidator : ISettingsValidator<FilesSettings>
{
    public IDictionary<string, string> Validate(FilesSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "File settings are required.");

            return errors;
        }

        if (!Enum.IsDefined(enumType: typeof(FileStorageType), value: settings.StorageType))
        {
            errors.Add(key: nameof(settings.StorageType), value: "StorageType must be a valid value.");
        }

        if (!string.IsNullOrEmpty(value: settings.RootPath) && !settings.RootPath.IsValidRootPath())
        {
            errors.Add(key: nameof(settings.RootPath), value: "RootPath must be a valid absolute path.");
        }

        if (string.IsNullOrEmpty(value: settings.ImportDirectory))
        {
            errors.Add(key: nameof(settings.ImportDirectory),
                value: "ImportDirectory must be provided and cannot be empty.");
        }
        else
        {
            if (!settings.ImportDirectory.IsValidRelativePath())
            {
                errors.Add(key: nameof(settings.ImportDirectory),
                    value: "ImportDirectory must be a valid relative path.");
            }
        }

        if (string.IsNullOrEmpty(value: settings.ReportsDirectory))
        {
            errors.Add(key: nameof(settings.ReportsDirectory),
                value: "ReportsDirectory must be provided and cannot be empty.");
        }
        else
        {
            if (!settings.ReportsDirectory.IsValidRelativePath())
            {
                errors.Add(key: nameof(settings.ReportsDirectory),
                    value: "ReportsDirectory must be a valid relative path.");
            }
        }

        return errors;
    }
}