using Expenso.Shared.Api.Swagger;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.UserPreferences.Api.Swagger.SchemaFilters.GetPreference.Response;

internal sealed class GetPreferenceResponseFinancePreferenceSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;

    public GetPreferenceResponseFinancePreferenceSchemaFilter(ISchemaDescriptor schemaDescriptor)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(GetPreferenceResponseFinancePreference))
        {
            return;
        }

        _schemaDescriptor.ConfigureProperty<GetPreferenceResponseFinancePreference>(schema: schema,
            propertyExpression: x => x.AllowAddFinancePlanReviewers,
            description: "Indicates whether the user is allowed to add finance plan reviewers",
            swaggerType: SwaggerTypes.Boolean, example: new OpenApiBoolean(value: true), swaggerFormat: null);

        _schemaDescriptor.ConfigureProperty<GetPreferenceResponseFinancePreference>(schema: schema,
            propertyExpression: x => x.AllowAddFinancePlanSubOwners,
            description: "Indicates whether the user is allowed to add finance plan sub owners",
            swaggerType: SwaggerTypes.Boolean, example: new OpenApiBoolean(value: true), swaggerFormat: null);

        _schemaDescriptor.ConfigureProperty<GetPreferenceResponseFinancePreference>(schema: schema,
            propertyExpression: x => x.MaxNumberOfFinancePlanReviewers,
            description: "The maximum number of finance plan reviewers that the user is allowed to add",
            swaggerType: SwaggerTypes.Integer, example: new OpenApiInteger(value: 2),
            swaggerFormat: SwaggerFormats.Int32);

        _schemaDescriptor.ConfigureProperty<GetPreferenceResponseFinancePreference>(schema: schema,
            propertyExpression: x => x.MaxNumberOfSubFinancePlanSubOwners,
            description: "The maximum number of sub finance plan sub owners that the user is allowed to add",
            swaggerType: SwaggerTypes.Integer, example: new OpenApiInteger(value: 5),
            swaggerFormat: SwaggerFormats.Int32);
    }
}