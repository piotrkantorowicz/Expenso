using Expenso.Shared.Database.EfCore.NpSql.DbContexts;

namespace Expenso.UserPreferences.Core.Persistence.EfCore;

internal sealed class UserPreferencesDbContextFactory : NpsqlDbContextFactory<UserPreferencesDbContext>;