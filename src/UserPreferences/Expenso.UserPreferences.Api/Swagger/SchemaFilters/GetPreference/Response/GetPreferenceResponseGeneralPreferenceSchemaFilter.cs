using Expenso.Shared.Api.Swagger;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.UserPreferences.Api.Swagger.SchemaFilters.GetPreference.Response;

internal sealed class GetPreferenceResponseGeneralPreferenceSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;

    public GetPreferenceResponseGeneralPreferenceSchemaFilter(ISchemaDescriptor schemaDescriptor)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(GetPreferenceResponseGeneralPreference))
        {
            return;
        }

        _schemaDescriptor.ConfigureProperty<GetPreferenceResponseGeneralPreference>(schema: schema,
            propertyExpression: x => x.UseDarkMode, description: "Indicates whether the user prefers dark mode",
            swaggerType: SwaggerTypes.Boolean, example: new OpenApiBoolean(value: true));
    }
}