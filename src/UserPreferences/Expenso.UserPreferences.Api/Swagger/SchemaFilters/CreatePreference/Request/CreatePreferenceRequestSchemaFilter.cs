using Expenso.Shared.Api.Swagger;
using Expenso.Shared.Commands.Validation;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Requests;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.UserPreferences.Api.Swagger.SchemaFilters.CreatePreference.Request;

internal sealed class CreatePreferenceRequestSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;
    private readonly ICommandValidator<CreatePreferenceCommand> _validator;

    public CreatePreferenceRequestSchemaFilter(ICommandValidator<CreatePreferenceCommand> validator,
        ISchemaDescriptor schemaDescriptor)
    {
        _validator = validator ?? throw new ArgumentNullException(paramName: nameof(validator));
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(CreatePreferenceRequest))
        {
            return;
        }

        IReadOnlyDictionary<string, CommandValidationRule<CreatePreferenceCommand>[]> validationMetadata =
            _validator.GetValidationMetadata();

        _schemaDescriptor.ConfigureProperty<CreatePreferenceCommand, CreatePreferenceRequest>(schema: schema,
            validationMetadata: validationMetadata, propertyExpression: x => x.UserId,
            description: "The unique identifier for the user", swaggerType: SwaggerTypes.String,
            swaggerFormat: SwaggerFormats.Uuid,
            example: new OpenApiString(value: "4c7a9020-398f-4d42-8cb4-cac1477a49ac"));
    }
}