using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.Api.Tests.E2E.TestData.DocumentManagement;
using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.E2E;

[SetUpFixture]
internal sealed class WebAppTestSetup
{
    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        using IServiceScope scope = WebApp.Instance.ServiceProvider.CreateScope();
        ICommandDispatcher commandDispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();

        IMessageContextFactory messageContextFactory =
            scope.ServiceProvider.GetRequiredService<IMessageContextFactory>();

        await PreferencesDataInitializer.Initialize(commandDispatcher: commandDispatcher,
            messageContextFactory: messageContextFactory, cancellationToken: default);

        await BudgetPermissionDataInitializer.Initialize(commandDispatcher: commandDispatcher,
            messageContextFactory: messageContextFactory, cancellationToken: default);

        await DocumentManagementDataInitializer.Initialize(commandDispatcher: commandDispatcher,
            messageContextFactory: messageContextFactory, cancellationToken: default);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        WebApp.Instance.Destroy();
    }
}