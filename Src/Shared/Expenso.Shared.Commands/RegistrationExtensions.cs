using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Commands.Validations;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands;

public static class RegistrationExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

        services.Scan(selector =>
            selector
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(ICommandValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.Scan(selector =>
            selector
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.Scan(selector =>
            selector
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.Decorate(typeof(ICommandHandler<>), typeof(Decorators.CommandHandlerValidationDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(Decorators.CommandHandlerValidationDecorator<,>));

        return services;
    }
}