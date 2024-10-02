using Expenso.Shared.Api.ProblemDetails.Details;

using Humanizer;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.Api.Configuration.Swagger;

internal sealed class ConflictErrorProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(ConflictErrorProblemDetails))
        {
            return;
        }

        ConflictErrorProblemDetails conflictErrorProblemDetails = new();

        schema.Properties[key: nameof(ConflictErrorProblemDetails.Title).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The title of the conflict error",
            Example = new OpenApiString(value: conflictErrorProblemDetails.Title)
        };

        schema.Properties[key: nameof(ConflictErrorProblemDetails.Type).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The type of the conflict error",
            Example = new OpenApiString(value: conflictErrorProblemDetails.Type)
        };

        schema.Properties[key: nameof(ConflictErrorProblemDetails.Detail).Camelize()] = new OpenApiSchema
        {
            Type = "object",
            Description = "The details of the conflict error",
            Example = new OpenApiString(
                value: "A conflict occurred while trying to save changes to the database. The record already exists.")
        };
    }
}