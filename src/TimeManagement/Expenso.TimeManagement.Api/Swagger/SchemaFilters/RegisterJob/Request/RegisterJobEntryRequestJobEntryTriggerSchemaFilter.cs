using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.Api.Swagger;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Proxy.DTO.RegisterJob.Requests;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.TimeManagement.Api.Swagger.SchemaFilters.RegisterJob.Request;

internal sealed class RegisterJobEntryRequestJobEntryTriggerSchemaFilter : ISchemaFilter
{
    private readonly IClock _clock;
    private readonly ISchemaDescriptor _schemaDescriptor;
    private readonly ISerializer _serializer;
    private readonly ICommandValidator<RegisterJobEntryCommand> _validator;

    public RegisterJobEntryRequestJobEntryTriggerSchemaFilter(ISchemaDescriptor schemaDescriptor,
        ICommandValidator<RegisterJobEntryCommand> validator, ISerializer serializer, IClock clock)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
        _validator = validator ?? throw new ArgumentNullException(paramName: nameof(validator));
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(RegisterJobEntryRequestJobEntryTrigger))
        {
            return;
        }

        IReadOnlyDictionary<string, CommandValidationRule<RegisterJobEntryCommand>[]> validationMetadata =
            _validator.GetValidationMetadata();

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequestJobEntryTrigger>(
            schema: schema, validationMetadata: validationMetadata, propertyExpression: x => x.EventType,
            description: "The type of the event that triggers the job entry", swaggerType: SwaggerTypes.String,
            example: new OpenApiString(value: typeof(BudgetPermissionRequestExpiredIntegrationEvent)
                .AssemblyQualifiedName));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequestJobEntryTrigger>(
            schema: schema, validationMetadata: validationMetadata, propertyExpression: x => x.EventData,
            description: "The data associated with the event that triggers the job entry",
            swaggerType: SwaggerTypes.String, example: new OpenApiString(value: _serializer.Serialize(
                value: new BudgetPermissionRequestExpiredIntegrationEvent(MessageContext: new MessageContext
                {
                    CorrelationId = Guid.NewGuid(),
                    MessageId = Guid.NewGuid(),
                    ModuleId = "BudgetSharing",
                    RequestedBy = Guid.NewGuid(),
                    Timestamp = _clock.UtcNow
                }, BudgetPermissionRequestId: Guid.NewGuid()))));
    }
}