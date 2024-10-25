using System.Reflection;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Validation;

public static class Extensions
{
    public static IServiceCollection AddCommandsValidations(this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        services.AddValidatorsFromAssemblies(assemblies: assemblies);

        services.TryDecorate(serviceType: typeof(ICommandHandler<>),
            decoratorType: typeof(CommandHandlerValidationDecorator<>));

        services.TryDecorate(serviceType: typeof(ICommandHandler<,>),
            decoratorType: typeof(CommandHandlerValidationDecorator<>));

        return services;
    }
}