using Expenso.Shared.Queries;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Internal.Queries.GetPreference;

public sealed record GetPreferenceInternalQuery(Guid UserId) : IQuery<GetPreferenceInternalResponse>;