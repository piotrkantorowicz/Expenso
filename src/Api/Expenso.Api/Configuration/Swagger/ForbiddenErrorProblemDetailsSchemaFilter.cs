using Expenso.Shared.Api.ProblemDetails.Details;

using Humanizer;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.Api.Configuration.Swagger;

internal sealed class ForbiddenErrorProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(ForbiddenProblemDetails))
        {
            return;
        }

        ForbiddenProblemDetails forbiddenProblemDetails = new();

        schema.Properties[key: nameof(ForbiddenProblemDetails.Title).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The title of the forbidden error",
            Example = new OpenApiString(value: forbiddenProblemDetails.Title)
        };

        schema.Properties[key: nameof(ForbiddenProblemDetails.Type).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The type of the forbidden error",
            Example = new OpenApiString(value: forbiddenProblemDetails.Type)
        };

        schema.Properties[key: nameof(ForbiddenProblemDetails.Detail).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The details of the forbidden error",
            Example = new OpenApiString(value: "The requested operation is forbidden.")
        };
    }
}