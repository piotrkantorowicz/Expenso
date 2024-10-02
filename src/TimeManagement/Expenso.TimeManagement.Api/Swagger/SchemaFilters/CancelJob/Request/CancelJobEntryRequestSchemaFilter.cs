using Expenso.Shared.Api.Swagger;
using Expenso.Shared.Commands.Validation;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.DTO.Requests;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.TimeManagement.Api.Swagger.SchemaFilters.CancelJob.Request;

internal sealed class CancelJobEntryRequestSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;
    private readonly ICommandValidator<CancelJobEntryCommand> _validator;

    public CancelJobEntryRequestSchemaFilter(ISchemaDescriptor schemaDescriptor,
        ICommandValidator<CancelJobEntryCommand> validator)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
        _validator = validator ?? throw new ArgumentNullException(paramName: nameof(validator));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(CancelJobEntryRequest))
        {
            return;
        }

        IReadOnlyDictionary<string, CommandValidationRule<CancelJobEntryCommand>[]> validationMetadata =
            _validator.GetValidationMetadata();

        _schemaDescriptor.ConfigureProperty<CancelJobEntryCommand, CancelJobEntryRequest>(schema: schema,
            validationMetadata: validationMetadata, propertyExpression: x => x.JobEntryId,
            description: "The job id to cancel", swaggerType: SwaggerTypes.String,
            example: new OpenApiString(value: "d3bb3715-0b83-4540-9abf-3d1dd62d9e41"));
    }
}