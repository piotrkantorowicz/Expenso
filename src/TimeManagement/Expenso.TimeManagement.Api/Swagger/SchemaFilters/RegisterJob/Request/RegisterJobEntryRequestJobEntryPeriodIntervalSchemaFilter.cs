using Expenso.Shared.Api.Swagger;
using Expenso.Shared.Commands.Validation;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Proxy.DTO.RegisterJob.Requests;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.TimeManagement.Api.Swagger.SchemaFilters.RegisterJob.Request;

internal sealed class RegisterJobEntryRequestJobEntryPeriodIntervalSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;
    private readonly ICommandValidator<RegisterJobEntryCommand> _validator;

    public RegisterJobEntryRequestJobEntryPeriodIntervalSchemaFilter(ISchemaDescriptor schemaDescriptor,
        ICommandValidator<RegisterJobEntryCommand> validator)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
        _validator = validator ?? throw new ArgumentNullException(paramName: nameof(validator));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(RegisterJobEntryRequestJobEntryPeriodInterval))
        {
            return;
        }

        IReadOnlyDictionary<string, CommandValidationRule<RegisterJobEntryCommand>[]> validationMetadata =
            _validator.GetValidationMetadata();

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequestJobEntryPeriodInterval>(
            schema: schema, validationMetadata: validationMetadata, propertyExpression: x => x.DayOfWeek,
            description: "The day of the week for the job entry period interval", swaggerType: SwaggerTypes.String,
            example: new OpenApiInteger(value: 3));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequestJobEntryPeriodInterval>(
            schema: schema, validationMetadata: validationMetadata, propertyExpression: x => x.Month,
            description: "The month for the job entry period interval", swaggerType: SwaggerTypes.String,
            example: new OpenApiInteger(value: 5));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequestJobEntryPeriodInterval>(
            schema: schema, validationMetadata: validationMetadata, propertyExpression: x => x.DayOfMonth,
            description: "The day of the month for the job entry period interval", swaggerType: SwaggerTypes.String,
            example: new OpenApiInteger(value: 23));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequestJobEntryPeriodInterval>(
            schema: schema, validationMetadata: validationMetadata, propertyExpression: x => x.Hour,
            description: "The hour for the job entry period interval", swaggerType: SwaggerTypes.String,
            example: new OpenApiInteger(value: 6));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequestJobEntryPeriodInterval>(
            schema: schema, validationMetadata: validationMetadata, propertyExpression: x => x.Minute,
            description: "The minute for the job entry period interval", swaggerType: SwaggerTypes.String,
            example: new OpenApiInteger(value: 15));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequestJobEntryPeriodInterval>(
            schema: schema, validationMetadata: validationMetadata, propertyExpression: x => x.Second,
            description: "The second for the job entry period interval", swaggerType: SwaggerTypes.String,
            example: new OpenApiInteger(value: 30));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryCommand, RegisterJobEntryRequestJobEntryPeriodInterval>(
            schema: schema, validationMetadata: validationMetadata, propertyExpression: x => x.UseSeconds,
            description: "Indicates whether seconds should be used in the cron expression",
            swaggerType: SwaggerTypes.String, example: new OpenApiBoolean(value: true));
    }
}