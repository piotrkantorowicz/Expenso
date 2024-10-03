using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;

public sealed record GetPreferenceQuery(
    IMessageContext MessageContext,
    Guid? PreferenceIdOrUserId = null,
    bool? ForCurrentUser = null,
    bool? IncludeFinancePreferences = null,
    bool? IncludeNotificationPreferences = null,
    bool? IncludeGeneralPreferences = null) : IQuery<GetPreferenceResponse>;