using Expenso.Shared.Api.Swagger;
using Expenso.Shared.Commands.Validation;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.UserPreferences.Api.Swagger.SchemaFilters.UpdatePreference.Request;

internal sealed class UpdatePreferenceRequestGeneralPreferenceSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;
    private readonly ICommandValidator<UpdatePreferenceCommand> _validator;

    public UpdatePreferenceRequestGeneralPreferenceSchemaFilter(ISchemaDescriptor schemaDescriptor,
        ICommandValidator<UpdatePreferenceCommand> validator)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
        _validator = validator ?? throw new ArgumentNullException(paramName: nameof(validator));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(UpdatePreferenceRequestGeneralPreference))
        {
            return;
        }

        IReadOnlyDictionary<string, CommandValidationRule<UpdatePreferenceCommand>[]> validationMetadata =
            _validator.GetValidationMetadata();

        _schemaDescriptor.ConfigureProperty<UpdatePreferenceCommand, UpdatePreferenceRequestGeneralPreference>(
            schema: schema, propertyExpression: x => x.UseDarkMode, validationMetadata: validationMetadata,
            description: "Indicates whether the user prefers dark mode", swaggerType: SwaggerTypes.Boolean,
            example: new OpenApiBoolean(value: true));
    }
}