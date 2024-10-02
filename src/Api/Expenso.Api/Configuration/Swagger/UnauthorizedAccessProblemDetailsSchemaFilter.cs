using Expenso.Shared.Api.ProblemDetails.Details;

using Humanizer;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.Api.Configuration.Swagger;

internal sealed class UnauthorizedAccessProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(UnauthorizedAccessProblemDetails))
        {
            return;
        }

        UnauthorizedAccessProblemDetails unauthorizedAccessProblemDetails = new();

        schema.Properties[key: nameof(UnauthorizedAccessProblemDetails.Title).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The title of the unauthorized access error",
            Example = new OpenApiString(value: unauthorizedAccessProblemDetails.Title)
        };

        schema.Properties[key: nameof(UnauthorizedAccessProblemDetails.Type).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The type of the unauthorized access error",
            Example = new OpenApiString(value: unauthorizedAccessProblemDetails.Type)
        };

        schema.Properties[key: nameof(UnauthorizedAccessProblemDetails.Detail).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The details of the unauthorized access error",
            Example = new OpenApiString(value: "The requested operation is unauthorized.")
        };
    }
}