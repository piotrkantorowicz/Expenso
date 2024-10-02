using Expenso.Shared.Api.Swagger;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.UserPreferences.Api.Swagger.SchemaFilters.GetPreference.Response;

internal sealed class GetPreferenceResponseNotificationPreferenceSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;

    public GetPreferenceResponseNotificationPreferenceSchemaFilter(ISchemaDescriptor schemaDescriptor)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(GetPreferenceResponseNotificationPreference))
        {
            return;
        }

        _schemaDescriptor.ConfigureProperty<GetPreferenceResponseNotificationPreference>(schema: schema,
            propertyExpression: x => x.SendFinanceReportEnabled,
            description: "Indicates whether the user prefers to receive email notifications",
            swaggerType: SwaggerTypes.Boolean, example: new OpenApiBoolean(value: true));

        _schemaDescriptor.ConfigureProperty<GetPreferenceResponseNotificationPreference>(schema: schema,
            propertyExpression: s => s.SendFinanceReportInterval,
            description: "The interval at which the user prefers to receive email notifications",
            swaggerType: SwaggerTypes.Boolean, example: new OpenApiInteger(value: 3));
    }
}