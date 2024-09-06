﻿using System.Text;

using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Proxy.DTO.Request;

using FluentAssertions;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandValidator;

internal sealed class Validate : RegisterJobEntryCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_CommandIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Command is required";
        string error = validationResult[key: "command"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_RegisterJobEntryRequestIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = null
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Register job entry request is required";
        string error = validationResult[key: nameof(RegisterJobEntryRequest)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_MaxRetriesIsPositive()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test, TestCase(arg: 0), TestCase(arg: -1), TestCase(arg: -50)]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxRetriesIsNegative(int maxRetries)
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
            {
                MaxRetries = maxRetries
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "MaxRetries must be a positive value";
        string error = validationResult[key: nameof(_registerJobEntryCommand.RegisterJobEntryRequest.MaxRetries)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_WhenIntervalIsNotEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
            {
                Interval = new RegisterJobEntryRequest_JobEntryPeriodInterval(),
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
            {
                Interval = new RegisterJobEntryRequest_JobEntryPeriodInterval(),
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
            {
                Interval = null,
                RunAt = _clockMock.Object.UtcNow.AddSeconds(seconds: 15)
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
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
                key: new StringBuilder()
                    .Append(value: nameof(_registerJobEntryCommand.RegisterJobEntryRequest.Interval))
                    .Append(value: '|')
                    .Append(value: nameof(_registerJobEntryCommand.RegisterJobEntryRequest.RunAt))
                    .ToString()];

        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenBothRunAtAndIntervalAreNotEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
            {
                Interval = new RegisterJobEntryRequest_JobEntryPeriodInterval(),
                RunAt = _clockMock.Object.UtcNow.AddSeconds(seconds: 15)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "RunAt and Interval cannot be used together";

        string error =
            validationResult[
                key: new StringBuilder()
                    .Append(value: nameof(_registerJobEntryCommand.RegisterJobEntryRequest.Interval))
                    .Append(value: '|')
                    .Append(value: nameof(_registerJobEntryCommand.RegisterJobEntryRequest.RunAt))
                    .ToString()];

        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerIsEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
            {
                JobEntryTriggers = new List<RegisterJobEntryRequest_JobEntryTrigger>()
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Job entry triggers are required";
        string error = validationResult[key: nameof(_registerJobEntryCommand.RegisterJobEntryRequest.JobEntryTriggers)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerDataIsEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
            {
                JobEntryTriggers =
                [
                    _registerJobEntryCommand.RegisterJobEntryRequest.JobEntryTriggers!.First() with
                    {
                        EventData = null
                    }
                ]
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Event data is required";
        string error = validationResult[key: nameof(JobEntryTrigger.EventData)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerTypeIsEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
            {
                JobEntryTriggers =
                [
                    _registerJobEntryCommand.RegisterJobEntryRequest.JobEntryTriggers!.First() with
                    {
                        EventType = null
                    }
                ]
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Event type is required";
        string error = validationResult[key: nameof(JobEntryTrigger.EventType)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_WhenEventTriggerDateIsNotSerializableToType()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
            {
                JobEntryTriggers =
                [
                    _registerJobEntryCommand.RegisterJobEntryRequest.JobEntryTriggers!.First() with
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
                key: new StringBuilder()
                    .Append(value: nameof(JobEntryTrigger.EventType))
                    .Append(value: '|')
                    .Append(value: nameof(JobEntryTrigger.EventData))
                    .ToString()];

        error.Should().Be(expected: expectedValidationMessage);
    }
}