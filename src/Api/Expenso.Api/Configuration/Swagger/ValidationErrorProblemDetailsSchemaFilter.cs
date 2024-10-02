using Expenso.Shared.Api.ProblemDetails.Details;
using Expenso.Shared.System.Types.TypesExtensions;

using Humanizer;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.Api.Configuration.Swagger;

internal sealed class ValidationErrorProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(ValidationErrorProblemDetails))
        {
            return;
        }

        ValidationErrorProblemDetails validationErrorProblemDetails = new();

        schema.Properties[key: nameof(ValidationErrorProblemDetails.Title).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The title of the validation error",
            Example = new OpenApiString(value: validationErrorProblemDetails.Title)
        };

        schema.Properties[key: nameof(ValidationErrorProblemDetails.Type).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The type of the validation error",
            Example = new OpenApiString(value: validationErrorProblemDetails.Type)
        };

        schema.Properties[key: nameof(ValidationErrorProblemDetails.Detail).Camelize()] = new OpenApiSchema
        {
            Type = "string",
            Description = "The details of the validation error",
            Example = ValidationErrorProblemDetailsDetailExample()
        };
    }

    private static OpenApiArray ValidationErrorProblemDetailsDetailExample()
    {
        OpenApiArray openApiArray = [];
        OpenApiObject openApiObject = new();
        openApiObject.AddTuple(item: ("property", new OpenApiString(value: "Id")));
        openApiObject.AddTuple(item: ("message", new OpenApiString(value: "The Id field is required.")));
        openApiArray.Add(item: openApiObject);

        return openApiArray;
    }
}