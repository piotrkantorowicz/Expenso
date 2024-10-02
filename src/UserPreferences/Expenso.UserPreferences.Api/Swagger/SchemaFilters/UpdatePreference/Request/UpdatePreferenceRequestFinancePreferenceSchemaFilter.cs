using Expenso.Shared.Api.Swagger;
using Expenso.Shared.Commands.Validation;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expenso.UserPreferences.Api.Swagger.SchemaFilters.UpdatePreference.Request;

internal sealed class UpdatePreferenceRequestFinancePreferenceSchemaFilter : ISchemaFilter
{
    private readonly ISchemaDescriptor _schemaDescriptor;
    private readonly ICommandValidator<UpdatePreferenceCommand> _validator;

    public UpdatePreferenceRequestFinancePreferenceSchemaFilter(ISchemaDescriptor schemaDescriptor,
        ICommandValidator<UpdatePreferenceCommand> validator)
    {
        _schemaDescriptor = schemaDescriptor ?? throw new ArgumentNullException(paramName: nameof(schemaDescriptor));
        _validator = validator ?? throw new ArgumentNullException(paramName: nameof(validator));
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(UpdatePreferenceRequestFinancePreference))
        {
            return;
        }

        IReadOnlyDictionary<string, CommandValidationRule<UpdatePreferenceCommand>[]> validationMetadata =
            _validator.GetValidationMetadata();

        _schemaDescriptor.ConfigureProperty<UpdatePreferenceCommand, UpdatePreferenceRequestFinancePreference>(
            schema: schema, propertyExpression: x => x.MaxNumberOfFinancePlanReviewers,
            validationMetadata: validationMetadata,
            description: "Indicates whether the user is allowed to add finance plan reviewers",
            swaggerType: SwaggerTypes.Boolean, example: new OpenApiBoolean(value: true), swaggerFormat: null);

        _schemaDescriptor.ConfigureProperty<UpdatePreferenceCommand, UpdatePreferenceRequestFinancePreference>(
            schema: schema, propertyExpression: x => x.AllowAddFinancePlanSubOwners,
            validationMetadata: validationMetadata,
            description: "Indicates whether the user is allowed to add finance plan sub owners",
            swaggerType: SwaggerTypes.Boolean, example: new OpenApiBoolean(value: true), swaggerFormat: null);

        _schemaDescriptor.ConfigureProperty<UpdatePreferenceCommand, UpdatePreferenceRequestFinancePreference>(
            schema: schema, propertyExpression: x => x.MaxNumberOfFinancePlanReviewers,
            validationMetadata: validationMetadata,
            description: "The maximum number of finance plan reviewers that the user is allowed to add",
            swaggerType: SwaggerTypes.Integer, example: new OpenApiInteger(value: 2),
            swaggerFormat: SwaggerFormats.Int32);

        _schemaDescriptor.ConfigureProperty<UpdatePreferenceCommand, UpdatePreferenceRequestFinancePreference>(
            schema: schema, propertyExpression: x => x.MaxNumberOfSubFinancePlanSubOwners,
            validationMetadata: validationMetadata,
            description: "The maximum number of sub finance plan sub owners that the user is allowed to add",
            swaggerType: SwaggerTypes.Integer, example: new OpenApiInteger(value: 5),
            swaggerFormat: SwaggerFormats.Int32);
    }
}