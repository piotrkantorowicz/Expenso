using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;

namespace Expenso.UserPreferences.Core.Application.Preferences.Mappings;

internal static class FinancePreferenceMap
{
    public static GetFinancePreferenceResponse MapToGetResponse(FinancePreference financePreference)
    {
        return new GetFinancePreferenceResponse(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static CreateFinancePreferenceResponse MapToCreateResponse(FinancePreference financePreference)
    {
        return new CreateFinancePreferenceResponse(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static FinancePreference MapToModel(UpdateFinancePreferenceRequest updateFinancePreference)
    {
        return FinancePreference.Create(updateFinancePreference.AllowAddFinancePlanSubOwners,
            updateFinancePreference.MaxNumberOfSubFinancePlanSubOwners,
            updateFinancePreference.AllowAddFinancePlanReviewers,
            updateFinancePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static GetFinancePreferenceInternalResponse MapToInternalGetRequest(FinancePreference financePreference)
    {
        return new GetFinancePreferenceInternalResponse(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static CreateFinancePreferenceInternalResponse MapToInternalCreateResponse(
        FinancePreference financePreference)
    {
        return new CreateFinancePreferenceInternalResponse(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static FinancePreferenceInternalContract MapToInternalContract(FinancePreference financePreference)
    {
        return new FinancePreferenceInternalContract(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }
}