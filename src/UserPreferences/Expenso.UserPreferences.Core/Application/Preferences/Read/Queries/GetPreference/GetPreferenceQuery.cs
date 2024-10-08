using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;

public sealed record GetPreferenceQuery(IMessageContext MessageContext, GetPreferenceRequest Payload)
    : IQuery<GetPreferenceResponse>;