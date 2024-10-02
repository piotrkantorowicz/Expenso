using Expenso.Shared.Api.ProblemDetails.Details;

using Humanizer;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.Api.Configuration.Swagger;

internal sealed class InternalServerErrorProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(InternalServerErrorProblemDetails))
        {
            return;
        }

        InternalServerErrorProblemDetails internalServerErrorProblemDetails = new();

        schema.Properties[key: nameof(InternalServerErrorProblemDetails.Title).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The title of the internal server error",
            Example = new OpenApiString(value: internalServerErrorProblemDetails.Title)
        };

        schema.Properties[key: nameof(InternalServerErrorProblemDetails.Type).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The type of the internal server error",
            Example = new OpenApiString(value: internalServerErrorProblemDetails.Type)
        };

        schema.Properties[key: nameof(InternalServerErrorProblemDetails.Detail).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The details of the internal server error",
            Example = new OpenApiString(value: internalServerErrorProblemDetails.Detail)
        };
    }
}