using System.Linq.Expressions;

using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Commands.Validation.Utils;

using Humanizer;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Expenso.Shared.Api.Swagger;

internal sealed class SwaggerSchemaDescriptor : ISchemaDescriptor
{
    public void ConfigureProperty<TType>(OpenApiSchema schema, Expression<Func<TType, object?>> propertyExpression,
        string description, string? swaggerType, IOpenApiAny? example, string? swaggerFormat = null) where TType : class
    {
        string property = GetMemberName(propertyExpression: propertyExpression);

        schema.Properties[key: property.Camelize()] = new OpenApiSchema
        {
            Type = swaggerType,
            Format = swaggerFormat,
            Description = description,
            Example = example
        };
    }

    public void ConfigureProperty<TCommand, TType>(OpenApiSchema schema,
        IReadOnlyDictionary<string, CommandValidationRule<TCommand>[]> validationMetadata,
        Expression<Func<TType, object?>> propertyExpression, string description, string? swaggerType,
        IOpenApiAny? example, string? swaggerFormat = null) where TCommand : class, ICommand where TType : class
    {
        string property = GetMemberName(propertyExpression: propertyExpression);

        (int? MinValue, int? MaxValue)? numberRange =
            GetNumberRange(validationMetadata: validationMetadata, property: property);

        (int? MinLenght, int? MaxLenght)? textLenght =
            GetTextLenght(validationMetadata: validationMetadata, property: property);

        schema.Properties[key: property.Camelize()] = new OpenApiSchema
        {
            Type = swaggerType,
            Format = swaggerFormat,
            Description = description,
            Minimum = numberRange?.MinValue ?? null,
            Maximum = numberRange?.MaxValue ?? null,
            MinLength = textLenght?.MinLenght ?? null,
            MaxLength = textLenght?.MaxLenght ?? null,
            Example = example
        };

        if (IsRequired(validationMetadata: validationMetadata, property: property))
        {
            schema.Required.Add(item: property.Camelize());
        }
    }

    private static bool IsRequired<TCommand>(
        IReadOnlyDictionary<string, CommandValidationRule<TCommand>[]> validationMetadata, string property)
        where TCommand : class, ICommand
    {
        return validationMetadata.ContainsKey(key: property) && validationMetadata[key: property]
            .Any(predicate: x => x is { ValidationType: ValidationTypes.Required, Value: true });
    }

    private static (int? MinLenght, int? MaxLenght)? GetTextLenght<TCommand>(
        IReadOnlyDictionary<string, CommandValidationRule<TCommand>[]> validationMetadata, string property)
        where TCommand : class, ICommand
    {
        if (!validationMetadata.TryGetValue(key: property, value: out CommandValidationRule<TCommand>[]? value))
        {
            return null;
        }

        int? minLenght = value
            .Where(predicate: x => x.ValidationType == ValidationTypes.MinTextLenght)
            .Select(selector: x => int.TryParse(s: x.Value?.ToString(), result: out int result) ? (int?)result : 0)
            .FirstOrDefault();

        int? maxLenght = validationMetadata[key: property]
            .Where(predicate: x => x.ValidationType == ValidationTypes.MaxTextLenght)
            .Select(selector: x => int.TryParse(s: x.Value?.ToString(), result: out int result) ? (int?)result : 0)
            .FirstOrDefault();

        return (minLenght, maxLenght);
    }

    private static (int? MinValue, int? MaxValue)? GetNumberRange<TCommand>(
        IReadOnlyDictionary<string, CommandValidationRule<TCommand>[]> validationMetadata, string property)
        where TCommand : class, ICommand
    {
        if (!validationMetadata.TryGetValue(key: property, value: out CommandValidationRule<TCommand>[]? value))
        {
            return null;
        }

        int? minLength = value
            .Where(predicate: x => x.ValidationType == ValidationTypes.MinValue)
            .Select(selector: x => int.TryParse(s: x.Value?.ToString(), result: out int result) ? (int?)result : null)
            .FirstOrDefault();

        int? maxLength = value
            .Where(predicate: x => x.ValidationType == ValidationTypes.MaxValue)
            .Select(selector: x => int.TryParse(s: x.Value?.ToString(), result: out int result) ? (int?)result : null)
            .FirstOrDefault();

        return (minLength, maxLength);
    }

    private static string GetMemberName<TType>(Expression<Func<TType, object?>> propertyExpression)
    {
        MemberExpression? memberExpression = propertyExpression.Body as MemberExpression ??
                                             ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;

        if (memberExpression is null)
        {
            throw new ArgumentException(message: "Expression must be a member expression",
                paramName: propertyExpression.ToString());
        }

        return memberExpression.Member.Name;
    }
}