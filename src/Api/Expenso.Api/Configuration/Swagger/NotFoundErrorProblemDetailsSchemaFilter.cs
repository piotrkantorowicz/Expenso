using Expenso.Shared.Api.ProblemDetails.Details;

using Humanizer;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.Api.Configuration.Swagger;

internal sealed class NotFoundErrorProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(NotFoundProblemDetails))
        {
            return;
        }

        NotFoundProblemDetails notFoundProblemDetails = new();

        schema.Properties[key: nameof(NotFoundProblemDetails.Title).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The title of the not found error",
            Example = new OpenApiString(value: notFoundProblemDetails.Title)
        };

        schema.Properties[key: nameof(NotFoundProblemDetails.Type).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The type of the not found error",
            Example = new OpenApiString(value: notFoundProblemDetails.Type)
        };

        schema.Properties[key: nameof(NotFoundProblemDetails.Detail).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The details of the not found error",
            Example = new OpenApiString(value: "The requested record was not found in the database.")
        };
    }
}