using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Validators.Cases;

internal sealed class ValidateUpdateAsync : PreferenceValidatorTestBase
{
    private readonly UpdatePreferenceDto _updatePreferenceDto = new(new UpdateFinancePreferenceDto(true, 2, true, 5),
        new UpdateNotificationPreferenceDto(true, 3), new UpdateGeneralPreferenceDto(false));

    [Test]
    public void Should_NotThrowValidationException_When_UserIdIsNotEmpty()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        PreferencesRepositoryMock
            .Setup(x => x.GetByIdAsync(preferenceIdOrUserId, true, default))
            .ReturnsAsync(Preference.CreateDefault(preferenceIdOrUserId));

        // Act
        // Assert
        Assert.DoesNotThrowAsync(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto, default));
    }

    [Test]
    public void Should_ThrowValidationException_When_UserIdIsEmpty()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.Empty;

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");

        exception
            ?.Details.Should()
            .Be(new StringBuilder()
                .AppendLine("PreferenceIdOrUserId: Preferences or user id cannot be empty.")
                .ToString());
    }

    [Test]
    public void Should_ThrowValidationException_When_PreferenceIsNull()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, null!, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");
        exception?.Details.Should().Be(new StringBuilder().Append("Preferences cannot be null.").ToString());
    }

    [Test]
    public void Should_ThrowValidationException_When_FinancePreferenceIsNull()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto with
            {
                FinancePreference = null!
            }, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");

        exception
            ?.Details.Should()
            .Be(new StringBuilder().AppendLine("Finance preferences cannot be null.").ToString());
    }

    [Test]
    public void Should_ThrowValidationException_When_MaxNumberOfFinancePlanReviewersIsNegative()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto with
            {
                FinancePreference = new UpdateFinancePreferenceDto(true, 5, true, -1)
            }, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");

        exception
            ?.Details.Should()
            .Be(new StringBuilder()
                .AppendLine("MaxNumberOfFinancePlanReviewers: Max number of finance plan reviewers cannot be negative.")
                .ToString());
    }

    [Test]
    public void Should_ThrowValidationException_When_MaxNumberOfFinancePlanReviewersIsGreaterThan10()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto with
            {
                FinancePreference = new UpdateFinancePreferenceDto(true, 5, true, 11)
            }, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");

        exception
            ?.Details.Should()
            .Be(new StringBuilder()
                .AppendLine(
                    "MaxNumberOfFinancePlanReviewers: Max number of finance plan reviewers cannot be greater than 10.")
                .ToString());
    }

    [Test]
    public void Should_ThrowValidationException_When_MaxNumberOfSubFinancePlanSubOwnersIsNegative()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto with
            {
                FinancePreference = new UpdateFinancePreferenceDto(true, -1, true, 5)
            }, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");

        exception
            ?.Details.Should()
            .Be(new StringBuilder()
                .AppendLine(
                    "MaxNumberOfSubFinancePlanSubOwners: Max number of sub finance plan sub owners cannot be negative.")
                .ToString());
    }

    [Test]
    public void Should_ThrowValidationException_When_MaxNumberOfSubFinancePlanSubOwnersIsGreaterThan5()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto with
            {
                FinancePreference = new UpdateFinancePreferenceDto(true, 6, true, 5)
            }, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");

        exception
            ?.Details.Should()
            .Be(new StringBuilder()
                .AppendLine(
                    "MaxNumberOfSubFinancePlanSubOwners: Max number of sub finance plan sub owners cannot be greater than 5.")
                .ToString());
    }

    [Test]
    public void Should_ThrowValidationException_When_NotificationPreferenceIsNull()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto with
            {
                NotificationPreference = null!
            }, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");

        exception
            ?.Details.Should()
            .Be(new StringBuilder().AppendLine("Notification preferences cannot be null.").ToString());
    }

    [Test]
    public void Should_ThrowValidationException_When_SendFinanceReportIntervalIsNegative()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto with
            {
                NotificationPreference = new UpdateNotificationPreferenceDto(true, -1)
            }, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");

        exception
            ?.Details.Should()
            .Be(new StringBuilder()
                .AppendLine("SendFinanceReportInterval: Send finance report interval cannot be negative.")
                .ToString());
    }

    [Test]
    public void Should_ThrowValidationException_When_SendFinanceReportIntervalIsGreaterThan10()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto with
            {
                NotificationPreference = new UpdateNotificationPreferenceDto(true, 32)
            }, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");

        exception
            ?.Details.Should()
            .Be(new StringBuilder()
                .AppendLine("SendFinanceReportInterval: Send finance report interval cannot be greater than 31.")
                .ToString());
    }

    [Test]
    public void Should_ThrowValidationException_When_GeneralPreferenceIsNull()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        // Act
        // Assert
        ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto with
            {
                GeneralPreference = null!
            }, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");

        exception
            ?.Details.Should()
            .Be(new StringBuilder().AppendLine("General preferences cannot be null.").ToString());
    }

    [Test]
    public void Should_ThrowConflictException_When_PreferenceDoesNotExist()
    {
        // Arrange
        Guid preferenceIdOrUserId = Guid.NewGuid();

        PreferencesRepositoryMock
            .Setup(x => x.GetByIdAsync(preferenceIdOrUserId, true, default))
            .ReturnsAsync((Preference?)null);

        // Act
        // Assert
        ConflictException? exception = Assert.ThrowsAsync<ConflictException>(() =>
            TestCandidate.ValidateUpdateAsync(preferenceIdOrUserId, _updatePreferenceDto, default));

        exception
            ?.Message.Should()
            .Be(new StringBuilder()
                .Append("User preferences for user with id ")
                .Append(preferenceIdOrUserId)
                .Append(" or with own id: ")
                .Append(preferenceIdOrUserId)
                .Append(" haven't been found.")
                .ToString());
    }
}