using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Validation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandsValidations(this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(ICommandValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.Decorate(typeof(ICommandHandler<>), typeof(CommandHandlerValidationDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(CommandHandlerValidationDecorator<,>));

        return services;
    }
}