using Expenso.Shared.Tests.Utils.UnitTests.Assertions;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Shared.DTO.Request;

using FluentValidation.Results;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandValidator;

[TestFixture]
internal sealed class Validate : RegisterJobEntryCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnEmptyValidationResult_When_MaxRetriesIsPositive()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand);

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_WhenIntervalIsNotEmpty()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                Interval = new RegisterJobEntryRequest_JobEntryPeriodInterval(),
                RunAt = null
            }
        });

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_WhenRunAtIsProvided()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                Interval = null,
                RunAt = _clockMock.Object.UtcNow.AddSeconds(seconds: 15)
            }
        });

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_RegisterJobEntryRequestIsNull()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = null
        });

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_registerJobEntryCommand.Payload),
            errorMessage: "The command payload must not be null.");
    }

    [Test, TestCase(arg: null), TestCase(arg: 0), TestCase(arg: -1), TestCase(arg: -50)]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxRetriesIsNegative(int maxRetries)
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                MaxRetries = maxRetries
            }
        });

        // Assert
        validationResult.AssertSingleError(
            propertyName:
            $"{nameof(_registerJobEntryCommand.Payload)}.{nameof(_registerJobEntryCommand.Payload.MaxRetries)}",
            errorMessage: "Max retries for job entry must be greater than 0.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenBothRunAtAndIntervalAreEmpty()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                Interval = null,
                RunAt = null
            }
        });

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_registerJobEntryCommand.Payload),
            errorMessage:
            "At least one value must be provided: Interval for periodic jobs or RunAt for single run jobs.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenBothRunAtAndIntervalAreNotEmpty()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                Interval = new RegisterJobEntryRequest_JobEntryPeriodInterval(),
                RunAt = _clockMock.Object.UtcNow.AddSeconds(seconds: 15)
            }
        });

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_registerJobEntryCommand.Payload),
            errorMessage: "RunAt and Interval cannot be used together.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenRunAtIsInvalid()
    {
        // Arrange
        DateTimeOffset runAt = _clockMock.Object.UtcNow.AddSeconds(seconds: -1);

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                RunAt = runAt,
                Interval = null
            }
        });

        // Assert
        validationResult.AssertSingleError(
            propertyName:
            $"{nameof(_registerJobEntryCommand.Payload)}.{nameof(_registerJobEntryCommand.Payload.RunAt)}",
            errorMessage: $"RunAt must be a future time. Provided: {runAt}.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenCronExpressionisInvalid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                RunAt = null,
                Interval = new RegisterJobEntryRequest_JobEntryPeriodInterval(DayOfWeek: 8)
            }
        });

        // Assert
        validationResult.AssertSingleError(
            propertyName:
            $"{nameof(_registerJobEntryCommand.Payload)}.{nameof(_registerJobEntryCommand.Payload.Interval)}",
            errorMessage:
            "Unable to parse provided interval, because of 8 is higher than the maximum allowable value for the [DayOfWeek] field. Value must be between 0 and 6 (all inclusive).");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerIsEmpty()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                JobEntryTriggers = new List<RegisterJobEntryRequest_JobEntryTrigger>()
            }
        });

        // Assert
        validationResult.AssertSingleError(
            propertyName:
            $"{nameof(_registerJobEntryCommand.Payload)}.{nameof(_registerJobEntryCommand.Payload.JobEntryTriggers)}",
            errorMessage: "Job entry triggers are required.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerDataIsEmpty()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                JobEntryTriggers =
                [
                    _registerJobEntryCommand.Payload.JobEntryTriggers!.First() with
                    {
                        EventData = null
                    }
                ]
            }
        });

        // Assert
        validationResult.AssertSingleError(
            propertyName:
            $"{nameof(_registerJobEntryCommand.Payload)}.{nameof(_registerJobEntryCommand.Payload.JobEntryTriggers)}[0].{nameof(JobEntryTrigger.EventData)}",
            errorMessage: "Event data is required.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerTypeIsEmpty()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                JobEntryTriggers =
                [
                    _registerJobEntryCommand.Payload.JobEntryTriggers!.First() with
                    {
                        EventType = null
                    }
                ]
            }
        });

        // Assert
        // Assert
        validationResult.AssertSingleError(
            propertyName:
            $"{nameof(_registerJobEntryCommand.Payload)}.{nameof(_registerJobEntryCommand.Payload.JobEntryTriggers)}[0].{nameof(JobEntryTrigger.EventType)}",
            errorMessage: "Event type is required.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerDateIsNotSerializableToType()
    {
        // Arrange
        const string invalidEventData = "InvalidEventType";
        _eventTypeResolver.Setup(expression: x => x.IsAllowable(invalidEventData)).Returns(value: false);

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _registerJobEntryCommand with
        {
            Payload = _registerJobEntryCommand.Payload! with
            {
                JobEntryTriggers =
                [
                    _registerJobEntryCommand.Payload.JobEntryTriggers!.First() with
                    {
                        EventType = invalidEventData
                    }
                ]
            }
        });

        // Assert
        validationResult.AssertSingleError(
            propertyName:
            $"{nameof(_registerJobEntryCommand.Payload)}.{nameof(_registerJobEntryCommand.Payload.JobEntryTriggers)}[0]",
            errorMessage: "EventData must be serializable to provided EventType.");
    }
}