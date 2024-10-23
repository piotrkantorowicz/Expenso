using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences;

public sealed record GetPreferencesQuery(IMessageContext MessageContext, GetPreferencesRequest? Payload)
    : IQuery<GetPreferencesResponse>;