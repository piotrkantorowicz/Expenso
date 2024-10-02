using Expenso.Shared.Api.ProblemDetails.Details;

using Humanizer;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.Api.Configuration.Swagger;

internal sealed class NotImplementedProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(NotImplementedProblemDetails))
        {
            return;
        }

        NotImplementedProblemDetails notImplementedProblemDetails = new();

        schema.Properties[key: nameof(NotImplementedProblemDetails.Title).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The title of the not implemented error",
            Example = new OpenApiString(value: notImplementedProblemDetails.Title)
        };

        schema.Properties[key: nameof(NotImplementedProblemDetails.Type).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The type of the not implemented error",
            Example = new OpenApiString(value: notImplementedProblemDetails.Type)
        };

        schema.Properties[key: nameof(NotImplementedProblemDetails.Detail).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The details of the not implemented error",
            Example = new OpenApiString(value: notImplementedProblemDetails.Detail)
        };
    }
}