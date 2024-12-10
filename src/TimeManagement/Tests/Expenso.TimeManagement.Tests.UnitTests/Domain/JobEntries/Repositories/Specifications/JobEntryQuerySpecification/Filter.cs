using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

using FluentAssertions;

namespace Expenso.TimeManagement.Tests.UnitTests.Domain.JobEntries.Repositories.Specifications.
    JobEntryQuerySpecification;

[TestFixture]
internal sealed class Filter : JobEntryQuerySpecificationTestBase
{
    [Test]
    public void Should_FilterByJobEntryId()
    {
        // Arrange
        Guid jobEntryId = _jobEntries.First().Id;

        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(JobEntryId: jobEntryId);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Should().HaveCount(expected: 1);
        jobEntries.First().Id.Should().Be(expected: jobEntryId);
    }

    [Test]
    public void Should_FilterByJobInstanceId()
    {
        // Arrange
        Guid jobInstanceId = _jobEntries.First().JobInstanceId;

        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(
                JobInstanceId: jobInstanceId);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Should().HaveCount(expected: 1);
        jobEntries.First().JobInstanceId.Should().Be(expected: jobInstanceId);
    }

    [Test]
    public void Should_FilterByJobEntryStatusIds()
    {
        // Arrange
        Guid[] jobEntryStatusIds = new[]
        {
            _jobEntries.First().JobEntryStatusId
        };

        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(
                JobEntryStatusIds: jobEntryStatusIds);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Should().HaveCount(expected: 1);
        jobEntries.First().JobEntryStatusId.Should().Be(expected: jobEntryStatusIds.First());
    }

    [Test]
    public void Should_FilterByMoreThanRetries()
    {
        // Arrange
        const int moreThanRetries = 1;

        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(
                MoreThanRetries: moreThanRetries);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Should().HaveCount(expected: 2);
        jobEntries.First().CurrentRetries.Should().BeGreaterOrEqualTo(expected: moreThanRetries);
    }

    [Test]
    public void Should_FilterByIsCompleted()
    {
        // Arrange
        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(IsCompleted: true);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Should().HaveCount(expected: 1);
        jobEntries.First().IsCompleted.Should().BeTrue();
    }

    [Test]
    public void Should_FilterByHasRunned()
    {
        // Arrange
        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(HasRunned: true);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Should().HaveCount(expected: 2);
        jobEntries.First().LastRun.Should().NotBeNull();
    }

    [Test]
    public void Should_FilterByIsActive()
    {
        // Arrange
        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(IsActive: true);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Should().HaveCount(expected: 1);
        jobEntries.First().Triggers.Should().NotBeEmpty();
    }

    [Test]
    public void Should_FilterByHasTriggers()
    {
        // Arrange
        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(HasTriggers: true);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Should().HaveCount(expected: 2);
        jobEntries.All(predicate: x => x.Triggers.Count > 0).Should().BeTrue();
    }
}