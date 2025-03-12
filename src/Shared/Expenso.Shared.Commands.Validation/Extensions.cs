using System.Reflection;

using Expenso.Shared.Commands.Validation.Validators;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Validation;

public static class Extensions
{
    public static IServiceCollection AddCommandsValidations(this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: [..assemblies, typeof(CommandValidator<>).Assembly])
                .AddClasses(action: c => c.AssignableTo(type: typeof(AbstractValidator<>)), publicOnly: false)
                .AsSelfWithInterfaces()
                .WithTransientLifetime());

        services.TryDecorate(serviceType: typeof(ICommandHandler<>),
            decoratorType: typeof(CommandHandlerValidationDecorator<>));

        services.TryDecorate(serviceType: typeof(ICommandHandler<,>),
            decoratorType: typeof(CommandHandlerValidationDecorator<,>));

        return services;
    }
}