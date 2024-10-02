using Expenso.Shared.Api.Swagger;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Responses;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.UserPreferences.Api.Swagger.SchemaFilters.CreatePreference.Response;

internal sealed class CreatePreferenceResponseSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;

    public CreatePreferenceResponseSchemaFilter(ISchemaDescriptor schemaDescriptor)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(CreatePreferenceResponse))
        {
            return;
        }

        _schemaDescriptor.ConfigureProperty<CreatePreferenceResponse>(schema: schema,
            propertyExpression: x => x.PreferenceId, description: "The unique identifier for the preference",
            swaggerType: SwaggerTypes.String, swaggerFormat: SwaggerFormats.Uuid,
            example: new OpenApiString(value: "f68546d6-fc1b-48cb-93d8-b96b29d8666d"));
    }
}