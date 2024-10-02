using System.Linq.Expressions;

using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Validation;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Expenso.Shared.Api.Swagger;

public interface ISchemaDescriptor
{
    void ConfigureProperty<TType>(OpenApiSchema schema, Expression<Func<TType, object?>> propertyExpression,
        string description, string? swaggerType, IOpenApiAny? example, string? swaggerFormat = null)
        where TType : class;

    void ConfigureProperty<TCommand, TType>(OpenApiSchema schema,
        IReadOnlyDictionary<string, CommandValidationRule<TCommand>[]> validationMetadata,
        Expression<Func<TType, object?>> propertyExpression, string description, string? swaggerType,
        IOpenApiAny? example, string? swaggerFormat = null) where TCommand : class, ICommand where TType : class;
}