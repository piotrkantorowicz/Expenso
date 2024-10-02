using Expenso.Shared.Api.Swagger;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Proxy.DTO.RegisterJob.Requests;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.TimeManagement.Api.Swagger.SchemaFilters.RegisterJob.Request;

internal sealed class RegisterJobEntryRequestSchemaFilter : ISchemaFilter
{
    private readonly IClock _clock;
    private readonly ISchemaDescriptor _schemaDescriptor;
    private readonly ICommandValidator<RegisterJobEntryCommand> _validator;

    public RegisterJobEntryRequestSchemaFilter(ISchemaDescriptor schemaDescriptor,
        ICommandValidator<RegisterJobEntryCommand> validator, IClock clock)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
        _validator = validator ?? throw new ArgumentNullException(paramName: nameof(validator));
        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(RegisterJobEntryRequest))
        {
            return;
        }

        IReadOnlyDictionary<string, CommandValidationRule<RegisterJobEntryCommand>[]> validationMetadata =
            _validator.GetValidationMetadata();

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequest>(schema: schema,
            validationMetadata: validationMetadata, propertyExpression: x => x.MaxRetries,
            description: "The maximum number of retries for the job", swaggerType: SwaggerTypes.String,
            example: new OpenApiInteger(value: 3));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequest>(schema: schema,
            validationMetadata: validationMetadata, propertyExpression: x => x.RunAt,
            description: "The time at which the job should run", swaggerType: SwaggerTypes.String,
            example: new OpenApiDateTime(value: _clock.UtcNow.AddMinutes(minutes: 30)));
    }
}