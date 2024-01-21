using Expenso.Shared.Queries;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Queries.GetPreference;

public sealed record GetPreferenceQuery(Guid? PreferenceId = null, Guid? UserId = null) : IQuery<GetPreferenceResponse>;