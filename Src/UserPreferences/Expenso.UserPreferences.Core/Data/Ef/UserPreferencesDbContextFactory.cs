using Expenso.Shared.Database.EfCore.NpSql.DbContexts;

namespace Expenso.UserPreferences.Core.Data.Ef;

internal sealed class UserPreferencesDbContextFactory : NpsqlDbContextFactory<UserPreferencesDbContext>;
