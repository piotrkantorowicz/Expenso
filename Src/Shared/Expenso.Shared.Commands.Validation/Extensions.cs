using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Validation;

public static class Extensions
{
    public static IServiceCollection AddCommandsValidations(this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(ICommandValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.TryDecorate(serviceType: typeof(ICommandHandler<>),
            decoratorType: typeof(CommandHandlerValidationDecorator<>));

        services.TryDecorate(serviceType: typeof(ICommandHandler<,>),
            decoratorType: typeof(CommandHandlerValidationDecorator<,>));

        return services;
    }
}