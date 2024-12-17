using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.Api.Tests.E2E.TestData.DocumentManagement;
using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.Api.Tests.E2E.TestData.TimeManagement;
using Expenso.DocumentManagement.Shared;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Shared;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

namespace Expenso.Api.Tests.E2E;

[SetUpFixture]
internal sealed class WebAppTestSetup
{
    [OneTimeSetUp]
    public async Task OneTimeSetupAsync()
    {
        using IServiceScope scope = WebApp.Instance.ServiceProvider.CreateScope();
        ICommandDispatcher commandDispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();

        IDocumentManagementProxy documentManagementProxy =
            scope.ServiceProvider.GetRequiredService<IDocumentManagementProxy>();

        ITimeManagementProxy timeManagementProxy = scope.ServiceProvider.GetRequiredService<ITimeManagementProxy>();
        IClock clock = scope.ServiceProvider.GetRequiredService<IClock>();

        await PreferencesDataInitializer.InitializeAsync(commandDispatcher: commandDispatcher, clock: clock,
            cancellationToken: default);

        await BudgetPermissionDataInitializer.InitializeAsync(commandDispatcher: commandDispatcher, clock: clock,
            cancellationToken: default);

        await DocumentManagementDataInitializer.InitializeAsync(documentManagementProxy: documentManagementProxy,
            clock: clock, cancellationToken: default);

        await TimeManagementDataInitializer.InitializeAsync(timeManagementProxy: timeManagementProxy, clock: clock,
            cancellationToken: default);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        EfCoreSettings databaseSettings = GetDatabaseSettings();
        await DropDatabaseAsync(databaseSettings: databaseSettings);
        WebApp.Instance.Destroy();
    }

    private static EfCoreSettings GetDatabaseSettings()
    {
        IConfiguration configuration = WebApp.Instance.ServiceProvider.GetRequiredService<IConfiguration>();
        EfCoreSettings databaseSettings = new();
        configuration.Bind(key: "EfCore", instance: databaseSettings);

        return databaseSettings;
    }

    private static async Task DropDatabaseAsync(EfCoreSettings databaseSettings,
        CancellationToken cancellationToken = default)
    {
        await using NpgsqlConnection connection =
            new(connectionString: databaseSettings.ConnectionParameters?.DefaultConnectionString);

        await connection.OpenAsync(cancellationToken: cancellationToken);

        string terminateConnectionsCommandText =
            $"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{databaseSettings.ConnectionParameters?.Database}';";

        await ExecuteNonQueryCommandAsync(connection: connection, commandText: terminateConnectionsCommandText,
            cancellationToken: cancellationToken);

        string dropDatabaseCommandText =
            $"DROP DATABASE IF EXISTS {databaseSettings.ConnectionParameters?.Database} WITH (FORCE);";

        await ExecuteNonQueryCommandAsync(connection: connection, commandText: dropDatabaseCommandText,
            cancellationToken: cancellationToken);

        await connection.CloseAsync();
    }

    private static async Task ExecuteNonQueryCommandAsync(NpgsqlConnection connection, string commandText,
        CancellationToken cancellationToken)
    {
        NpgsqlCommand command = new(cmdText: commandText, connection: connection);
        await command.ExecuteNonQueryAsync(cancellationToken: cancellationToken);
        await command.DisposeAsync();
    }
}