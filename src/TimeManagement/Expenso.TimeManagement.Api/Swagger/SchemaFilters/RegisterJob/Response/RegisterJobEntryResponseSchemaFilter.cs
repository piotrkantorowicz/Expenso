using Expenso.Shared.Api.Swagger;
using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Proxy.DTO.RegisterJob.Responses;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.TimeManagement.Api.Swagger.SchemaFilters.RegisterJob.Response;

internal sealed class RegisterJobEntryResponseSchemaFilter : ISchemaFilter
{
    private readonly IClock _clock;
    private readonly ISchemaDescriptor _schemaDescriptor;

    public RegisterJobEntryResponseSchemaFilter(ISchemaDescriptor schemaDescriptor, IClock clock)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(RegisterJobEntryResponse))
        {
            return;
        }

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryResponse>(schema: schema,
            propertyExpression: x => x.JobEntryId, description: "The unique identifier for the job entry",
            swaggerType: SwaggerTypes.String, swaggerFormat: SwaggerFormats.Uuid,
            example: new OpenApiString(value: "d3bb3715-0b83-4540-9abf-3d1dd62d9e41"));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryResponse>(schema: schema,
            propertyExpression: x => x.JobInstanceId, description: "The unique identifier for the job instance",
            swaggerType: SwaggerTypes.String, swaggerFormat: SwaggerFormats.Uuid,
            example: new OpenApiString(value: "d8fc5aed-cc40-4484-864f-945480daa236"));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryResponse>(schema: schema,
            propertyExpression: x => x.JobEntryStatusId, description: "The unique identifier for the job status",
            swaggerType: SwaggerTypes.String, swaggerFormat: SwaggerFormats.Uuid,
            example: new OpenApiString(value: "53b12b3e-1db8-4792-9e24-a4da5f3e5ba3"));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryResponse>(schema: schema,
            propertyExpression: x => x.CronExpression,
            description: "The cron expression that defines the schedule for the job", swaggerType: SwaggerTypes.String,
            example: new OpenApiString(value: "0 0 * * *"));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryResponse>(schema: schema, propertyExpression: x => x.RunAt,
            description: "The date and time at which the job is scheduled to run", swaggerType: SwaggerTypes.String,
            swaggerFormat: SwaggerFormats.DateTime, example: new OpenApiDateTime(value: _clock.UtcNow));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryResponse>(schema: schema,
            propertyExpression: x => x.CurrentRetries, description: "The number of times the job has been retried",
            swaggerType: SwaggerTypes.Integer, swaggerFormat: SwaggerFormats.Int32,
            example: new OpenApiInteger(value: 0));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryResponse>(schema: schema,
            propertyExpression: x => x.MaxRetries, description: "The maximum number of times the job can be retried",
            swaggerType: SwaggerTypes.Integer, swaggerFormat: SwaggerFormats.Int32,
            example: new OpenApiInteger(value: 3));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryResponse>(schema: schema,
            propertyExpression: x => x.IsCompleted, description: "Indicates whether the job has completed",
            swaggerType: SwaggerTypes.Boolean, example: new OpenApiBoolean(value: false));

        _schemaDescriptor.ConfigureProperty<RegisterJobEntryResponse>(schema: schema,
            propertyExpression: x => x.LastRun, description: "The date and time at which the job was last run",
            swaggerType: SwaggerTypes.String, swaggerFormat: SwaggerFormats.DateTime,
            example: new OpenApiDateTime(value: _clock.UtcNow.AddMinutes(minutes: -30)));
    }
}