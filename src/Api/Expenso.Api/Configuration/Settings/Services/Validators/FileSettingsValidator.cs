using Expenso.Shared.System.Configuration.Settings.Files;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class FilesSettingsValidator : AbstractValidator<FilesSettings>
{
    public FilesSettingsValidator()
    {
        RuleFor(expression: x => x).NotNull().WithMessage(errorMessage: "File settings are required.");

        RuleFor(expression: x => x.StorageType)
            .Must(predicate: storageType => Enum.IsDefined(enumType: typeof(FileStorageType), value: storageType))
            .WithMessage(errorMessage: "StorageType must be a valid value.");

        RuleFor(expression: x => x.RootPath)
            .Must(predicate: rootPath => string.IsNullOrWhiteSpace(value: rootPath) || rootPath.IsValidRootPath())
            .WithMessage(errorMessage: "RootPath must be a valid absolute path.");

        RuleFor(expression: x => x.ImportDirectory)
            .NotEmpty()
            .WithMessage(errorMessage: "ImportDirectory must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.ImportDirectory)
                .Must(predicate: importDirectory => importDirectory.IsValidRelativePath())
                .WithMessage(errorMessage: "ImportDirectory must be a valid relative path."));

        RuleFor(expression: x => x.ReportsDirectory)
            .NotEmpty()
            .WithMessage(errorMessage: "ReportsDirectory must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.ReportsDirectory)
                .Must(predicate: reportsDirectory => reportsDirectory.IsValidRelativePath())
                .WithMessage(errorMessage: "ReportsDirectory must be a valid relative path."));
    }
}