using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Validation;

public static class RegistrationExtensions
{
    public static IServiceCollection AddCommandsValidation(this IServiceCollection services)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(ICommandValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.Decorate(typeof(ICommandHandler<>), typeof(CommandHandlerValidationDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(CommandHandlerValidationDecorator<,>));

        return services;
    }
}