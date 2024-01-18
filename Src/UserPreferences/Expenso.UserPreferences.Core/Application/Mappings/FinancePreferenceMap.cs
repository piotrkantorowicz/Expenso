using Expenso.UserPreferences.Core.Application.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.Application.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Core.Application.Mappings;

internal static class FinancePreferenceMap
{
    public static FinancePreferenceDto MapToDto(FinancePreference financePreference)
    {
        return new FinancePreferenceDto(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static FinancePreferenceContract MapToContract(FinancePreference financePreference)
    {
        return new FinancePreferenceContract(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static FinancePreference MapToModel(UpdateFinancePreferenceDto updateFinancePreference)
    {
        return FinancePreference.Create(updateFinancePreference.AllowAddFinancePlanSubOwners,
            updateFinancePreference.MaxNumberOfSubFinancePlanSubOwners,
            updateFinancePreference.AllowAddFinancePlanReviewers,
            updateFinancePreference.MaxNumberOfFinancePlanReviewers);
    }
}