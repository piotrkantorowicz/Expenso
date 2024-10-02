using Expenso.Shared.Api.Swagger;
using Expenso.Shared.Commands.Validation;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.UserPreferences.Api.Swagger.SchemaFilters.UpdatePreference.Request;

internal sealed class UpdatePreferenceRequestNotificationPreferenceSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;
    private readonly ICommandValidator<UpdatePreferenceCommand> _validator;

    public UpdatePreferenceRequestNotificationPreferenceSchemaFilter(ISchemaDescriptor schemaDescriptor,
        ICommandValidator<UpdatePreferenceCommand> validator)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
        _validator = validator ?? throw new ArgumentNullException(paramName: nameof(validator));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(UpdatePreferenceRequestNotificationPreference))
        {
            return;
        }

        IReadOnlyDictionary<string, CommandValidationRule<UpdatePreferenceCommand>[]> validationMetadata =
            _validator.GetValidationMetadata();

        _schemaDescriptor.ConfigureProperty<UpdatePreferenceCommand, UpdatePreferenceRequestNotificationPreference>(
            schema: schema, validationMetadata: validationMetadata, propertyExpression: x => x.SendFinanceReportEnabled,
            description: "Indicates whether the user prefers to receive email notifications",
            swaggerType: SwaggerTypes.Boolean, example: new OpenApiBoolean(value: true));

        _schemaDescriptor.ConfigureProperty<UpdatePreferenceCommand, UpdatePreferenceRequestNotificationPreference>(
            schema: schema, validationMetadata: validationMetadata,
            propertyExpression: x => x.SendFinanceReportInterval,
            description: "The interval at which the user prefers to receive email notifications",
            swaggerType: SwaggerTypes.Boolean, example: new OpenApiInteger(value: 3));
    }
}