using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Shared.System.Configuration.Validators;

public interface ISettingsValidator<in TSettings> where TSettings : class, ISettings
{
    IDictionary<string, string> Validate(TSettings settings);
}