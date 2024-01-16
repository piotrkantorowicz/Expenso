using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Core.Repositories;

namespace Expenso.UserPreferences.Core.Validators;

internal sealed class PreferenceValidator(IPreferencesRepository preferencesRepository) : IPreferenceValidator
{
    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    public async Task ValidateCreateAsync(Guid userId, CancellationToken cancellationToken)
    {
        Dictionary<string, string> errors = new();

        if (userId == Guid.Empty)
        {
            errors.Add(nameof(userId), "User id cannot be empty.");
            
            throw new ValidationException(errors);
        }

        Preference? dbUserPreferences = await _preferencesRepository.GetByUserIdAsync(userId, true, cancellationToken);

        if (dbUserPreferences is not null)
        {
            throw new ConflictException($"Preferences for user with id {userId} already exists.");
        }
    }

    public async Task<Preference> ValidateUpdateAsync(Guid preferenceIdOrUserId, UpdatePreferenceDto preferenceDto,
        CancellationToken cancellationToken)
    {
        if (preferenceDto is null)
        {
            throw new ValidationException("Preferences cannot be null.");
        }

        Dictionary<string, string> errors = new();

        if (preferenceIdOrUserId == Guid.Empty)
        {
            errors.Add(nameof(preferenceIdOrUserId), "Preferences or user id cannot be empty.");
        }

        if (preferenceDto.FinancePreference is null)
        {
            throw new ValidationException("Finance preferences cannot be null.", errors);
        }

        switch (preferenceDto.FinancePreference.MaxNumberOfFinancePlanReviewers)
        {
            case < 0:
                errors.Add(nameof(preferenceDto.FinancePreference.MaxNumberOfFinancePlanReviewers),
                    "Max number of finance plan reviewers cannot be negative.");

                break;
            case > 10:
                errors.Add(nameof(preferenceDto.FinancePreference.MaxNumberOfFinancePlanReviewers),
                    "Max number of finance plan reviewers cannot be greater than 10.");

                break;
        }

        switch (preferenceDto.FinancePreference.MaxNumberOfSubFinancePlanSubOwners)
        {
            case < 0:
                errors.Add(nameof(preferenceDto.FinancePreference.MaxNumberOfSubFinancePlanSubOwners),
                    "Max number of sub finance plan sub owners cannot be negative.");

                break;
            case > 5:
                errors.Add(nameof(preferenceDto.FinancePreference.MaxNumberOfSubFinancePlanSubOwners),
                    "Max number of sub finance plan sub owners cannot be greater than 5.");

                break;
        }

        if (preferenceDto.NotificationPreference is null)
        {
            throw new ValidationException("Notification preferences cannot be null.", errors);
        }

        switch (preferenceDto.NotificationPreference.SendFinanceReportInterval)
        {
            case < 0:
                errors.Add(nameof(preferenceDto.NotificationPreference.SendFinanceReportInterval),
                    "Send finance report interval cannot be negative.");

                break;
            case > 31:
                errors.Add(nameof(preferenceDto.NotificationPreference.SendFinanceReportInterval),
                    "Send finance report interval cannot be greater than 31.");

                break;
        }

        if (preferenceDto.GeneralPreference is null)
        {
            throw new ValidationException("General preferences cannot be null.", errors);
        }

        if (errors.Count != 0)
        {
            throw new ValidationException(errors);
        }

        Preference? dbUserPreferences =
            await _preferencesRepository.GetByIdAsync(new PreferenceId(preferenceIdOrUserId), true, cancellationToken) ??
            await _preferencesRepository.GetByUserIdAsync(new UserId(preferenceIdOrUserId), true, cancellationToken);

        if (dbUserPreferences is null)
        {
            throw new ConflictException(
                $"User preferences for user with id {preferenceIdOrUserId} or with own id: {preferenceIdOrUserId} haven't been found.");
        }

        return dbUserPreferences;
    }
}