using Expenso.Shared.Database.EfCore.Npsql.DbContexts;

namespace Expenso.UserPreferences.Core.Persistence.EfCore;

internal sealed class UserPreferencesDbContextFactory : NpsqlDbContextFactory<UserPreferencesDbContext>;