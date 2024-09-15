using Expenso.Shared.System.Configuration.Settings.Files;
using Expenso.Shared.System.Configuration.Validators;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class FilesSettingsValidator : ISettingsValidator<FilesSettings>
{
    public IDictionary<string, string> Validate(FilesSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings), value: "Settings are required");

            return errors;
        }

        if (!Enum.IsDefined(enumType: typeof(FileStorageType), value: settings.StorageType))
        {
            errors.Add(key: nameof(settings.StorageType), value: "StorageType must be a valid value");
        }

        if (string.IsNullOrEmpty(value: settings.ImportDirectory))
        {
            errors.Add(key: nameof(settings.ImportDirectory),
                value: "ImportDirectory must be provided and cannot be empty");
        }

        if (string.IsNullOrEmpty(value: settings.ReportsDirectory))
        {
            errors.Add(key: nameof(settings.ReportsDirectory),
                value: "ReportsDirectory must be provided and cannot be empty");
        }

        return errors;
    }
}