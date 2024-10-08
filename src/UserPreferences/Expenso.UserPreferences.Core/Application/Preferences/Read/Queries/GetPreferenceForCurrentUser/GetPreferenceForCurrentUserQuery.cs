using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser;

public sealed record GetPreferenceForCurrentUserQuery(
    IMessageContext MessageContext,
    GetPreferenceForCurrentUserRequest Payload) : IQuery<GetPreferenceForCurrentUserResponse>;