using System.Text;

using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Proxy.DTO.Request;

using FluentAssertions;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.RegisterJobCommandValidator;

internal sealed class Validate : RegisterJobCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_CommandIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Command is required";
        string error = validationResult["command"];
        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AddJobEntryRequestIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = null
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Add job entry request is required";
        string error = validationResult[nameof(AddJobEntryRequest)];
        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_MaxRetriesIsPositive()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-50)]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxRetriesIsNegative(int maxRetries)
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = _registerJobCommand.AddJobEntryRequest! with
            {
                MaxRetries = maxRetries
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "MaxRetries must be a positive value";
        string error = validationResult[nameof(_registerJobCommand.AddJobEntryRequest.MaxRetries)];
        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_WhenIntervalIsNotEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = _registerJobCommand.AddJobEntryRequest! with
            {
                Interval = new AddJobEntryRequest_JobEntryPeriodInterval(),
                RunAt = null
            }
        });

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_WhenIntervalIsEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = _registerJobCommand.AddJobEntryRequest! with
            {
                Interval = new AddJobEntryRequest_JobEntryPeriodInterval(),
                RunAt = null
            }
        });

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_WhenRunAtIsProvided()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = _registerJobCommand.AddJobEntryRequest! with
            {
                Interval = null,
                RunAt = _clockMock.Object.UtcNow.AddSeconds(15)
            }
        });

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenBothRunAtAndIntervalAreEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = _registerJobCommand.AddJobEntryRequest! with
            {
                Interval = null,
                RunAt = null
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();

        const string expectedValidationMessage =
            "At least one value must be provide Interval for periodic jobs or RunAt for single run jobs";

        string error =
            validationResult[
                new StringBuilder()
                    .Append(nameof(_registerJobCommand.AddJobEntryRequest.Interval))
                    .Append('|')
                    .Append(nameof(_registerJobCommand.AddJobEntryRequest.RunAt))
                    .ToString()];

        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenBothRunAtAndIntervalAreNotEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = _registerJobCommand.AddJobEntryRequest! with
            {
                Interval = new AddJobEntryRequest_JobEntryPeriodInterval(),
                RunAt = _clockMock.Object.UtcNow.AddSeconds(15)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "RunAt and Interval cannot be used together";

        string error =
            validationResult[
                new StringBuilder()
                    .Append(nameof(_registerJobCommand.AddJobEntryRequest.Interval))
                    .Append('|')
                    .Append(nameof(_registerJobCommand.AddJobEntryRequest.RunAt))
                    .ToString()];

        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerIsEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = _registerJobCommand.AddJobEntryRequest! with
            {
                JobEntryTriggers = new List<AddJobEntryRequest_JobEntryTrigger>()
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Job entry triggers are required";
        string error = validationResult[nameof(_registerJobCommand.AddJobEntryRequest.JobEntryTriggers)];
        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerDataIsEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = _registerJobCommand.AddJobEntryRequest! with
            {
                JobEntryTriggers =
                [
                    _registerJobCommand.AddJobEntryRequest.JobEntryTriggers!.First() with
                    {
                        EventData = null
                    }
                ]
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Event data is required";
        string error = validationResult[nameof(JobEntryTrigger.EventData)];
        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerTypeIsEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = _registerJobCommand.AddJobEntryRequest! with
            {
                JobEntryTriggers =
                [
                    _registerJobCommand.AddJobEntryRequest.JobEntryTriggers!.First() with
                    {
                        EventType = null
                    }
                ]
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Event type is required";
        string error = validationResult[nameof(JobEntryTrigger.EventType)];
        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerDateIsNotSerializableToType()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_registerJobCommand with
        {
            AddJobEntryRequest = _registerJobCommand.AddJobEntryRequest! with
            {
                JobEntryTriggers =
                [
                    _registerJobCommand.AddJobEntryRequest.JobEntryTriggers!.First() with
                    {
                        EventType = typeof(int).AssemblyQualifiedName
                    }
                ]
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "EventData must be serializable to provided EventType";

        string error =
            validationResult[
                new StringBuilder()
                    .Append(nameof(JobEntryTrigger.EventType))
                    .Append('|')
                    .Append(nameof(JobEntryTrigger.EventData))
                    .ToString()];

        error.Should().Be(expectedValidationMessage);
    }
}