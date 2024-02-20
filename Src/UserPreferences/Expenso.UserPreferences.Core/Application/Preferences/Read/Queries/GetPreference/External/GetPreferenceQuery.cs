using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.External;

public sealed record GetPreferenceQuery(
    IMessageContext MessageContext,
    Guid? UserId = null,
    bool? IncludeFinancePreferences = null,
    bool? IncludeNotificationPreferences = null,
    bool? IncludeGeneralPreferences = null) : IQuery<GetPreferenceResponse>;