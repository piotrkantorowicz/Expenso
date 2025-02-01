using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using Expenso.Api.Tests.E2E.Configuration;
using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.Api.Tests.E2E.TestData.DocumentManagement;
using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.Api.Tests.E2E.TestData.TimeManagement;
using Expenso.BudgetSharing.Shared;
using Expenso.DocumentManagement.Shared;
using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Time;
using Expenso.TimeManagement.Shared;
using Expenso.UserPreferences.Shared;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E;

[SetUpFixture]
internal sealed class WebAppTestSetup
{
    [OneTimeSetUp]
    public async Task OneTimeSetupAsync()
    {
        try
        {
            using IServiceScope scope = WebApp.Instance.ServiceProvider.CreateScope();
            await InitializeTestDataAsync(scope: scope);
        }
        catch (Exception ex)
        {
            await TestContext.Error.WriteLineAsync(value: $"Web app setup failed: {ex}");

            throw;
        }
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        try
        {
            EfCoreSettings databaseSettings = GetDatabaseSettings();
            await DropDatabaseAsync(databaseSettings: databaseSettings);
            WebApp.Instance.Destroy();
        }
        catch (Exception ex)
        {
            await TestContext.Error.WriteLineAsync(value: $"Web app teardown failed: {ex}");

            throw;
        }
    }

    private static async Task InitializeTestDataAsync(IServiceScope scope)
    {
        IDocumentManagementProxy documentManagementProxy =
            scope.ServiceProvider.GetRequiredService<IDocumentManagementProxy>();

        ITimeManagementProxy timeManagementProxy = scope.ServiceProvider.GetRequiredService<ITimeManagementProxy>();
        IUserPreferencesProxy userPreferencesProxy = scope.ServiceProvider.GetRequiredService<IUserPreferencesProxy>();
        IBudgetSharingProxy budgetSharingProxy = scope.ServiceProvider.GetRequiredService<IBudgetSharingProxy>();
        IClock clock = scope.ServiceProvider.GetRequiredService<IClock>();

        await RunInitializeActionAsync(scope: scope, moduleName: ModuleNames.UserPreferencesModule,
            action: () => PreferencesDataInitializer.InitializeAsync(clock: clock,
                userPreferencesProxy: userPreferencesProxy, cancellationToken: default));

        await RunInitializeActionAsync(scope: scope, moduleName: ModuleNames.BudgetSharingModule,
            action: () => BudgetSharingDataInitializer.InitializeAsync(clock: clock,
                budgetSharingProxy: budgetSharingProxy, cancellationToken: default));

        await RunInitializeActionAsync(scope: scope, moduleName: ModuleNames.DocumentManagementModule,
            action: () =>
                DocumentManagementDataInitializer.InitializeAsync(documentManagementProxy: documentManagementProxy,
                    clock: clock, cancellationToken: default));

        await RunInitializeActionAsync(scope: scope, moduleName: ModuleNames.TimeManagementModule,
            action: () => TimeManagementDataInitializer.InitializeAsync(timeManagementProxy: timeManagementProxy,
                clock: clock, cancellationToken: default));
    }

    private static async Task RunInitializeActionAsync(IServiceScope scope, string moduleName, Func<Task> action)
    {
        IHttpContextAccessor? httpContextAccessor = null;

        try
        {
            httpContextAccessor = SetupHttpContext(scope: scope, moduleName: moduleName);
            await action();
        }
        finally
        {
            if (httpContextAccessor != null)
            {
                httpContextAccessor.HttpContext = null;
            }
        }
    }

    [SuppressMessage(category: "Usage", checkId: "ASP0019:Suggest using IHeaderDictionary.Append or the indexer",
        Justification = "Using the indexer for simplicity.")]
    private static IHttpContextAccessor SetupHttpContext(IServiceScope scope, string moduleName)
    {
        IHttpContextAccessor httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

        httpContextAccessor.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(identity: new ClaimsIdentity(
                claims: TestBase.Claims.Select(selector: x =>
                    new Claim(type: x.Key, value: x.Value?.ToString() ?? string.Empty)),
                authenticationType: "Granted")),
            Request =
            {
                Headers =
                {
                    { "CorrelationId", Guid.CreateVersion7().ToString() },
                    { "ModuleId", moduleName }
                }
            }
        };

        return httpContextAccessor;
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