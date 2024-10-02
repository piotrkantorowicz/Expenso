using Expenso.Shared.Api.Swagger;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.UserPreferences.Api.Swagger.SchemaFilters.GetPreference.Response;

internal sealed class GetPreferenceResponseSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;

    public GetPreferenceResponseSchemaFilter(ISchemaDescriptor schemaDescriptor)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(GetPreferenceResponse))
        {
            return;
        }

        _schemaDescriptor.ConfigureProperty<GetPreferenceResponse>(schema: schema, propertyExpression: x => x.Id,
            description: "The unique identifier for the preference", swaggerType: SwaggerTypes.String,
            swaggerFormat: SwaggerFormats.Uuid,
            example: new OpenApiString(value: "f68546d6-fc1b-48cb-93d8-b96b29d8666d"));

        _schemaDescriptor.ConfigureProperty<GetPreferenceResponse>(schema: schema, propertyExpression: x => x.UserId,
            description: "The unique identifier for the user", swaggerType: SwaggerTypes.String,
            swaggerFormat: SwaggerFormats.Uuid,
            example: new OpenApiString(value: "4c7a9020-398f-4d42-8cb4-cac1477a49ac"));
    }
}