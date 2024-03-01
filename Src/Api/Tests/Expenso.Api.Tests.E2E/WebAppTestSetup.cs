using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
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

        await PreferencesDataProvider.Initialize(commandDispatcher, messageContextFactory, default);
        await BudgetPermissionRequestsDataProvider.Initialize(commandDispatcher, messageContextFactory, default);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        WebApp.Instance.Destroy();
    }
}