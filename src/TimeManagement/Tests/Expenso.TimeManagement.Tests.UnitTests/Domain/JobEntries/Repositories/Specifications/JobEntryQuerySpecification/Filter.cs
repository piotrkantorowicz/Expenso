using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

using NUnit.Framework;

using Shouldly;

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
        jobEntries.Count.ShouldBe(expected: 1);
        jobEntries.First().Id.ShouldBe(expected: jobEntryId);
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
        jobEntries.Count.ShouldBe(expected: 1);
        jobEntries.First().JobInstanceId.ShouldBe(expected: jobInstanceId);
    }

    [Test]
    public void Should_FilterByJobEntryStatusIds()
    {
        // Arrange
        Guid[] jobEntryStatusIds =
        [
            _jobEntries[index: 0].JobEntryStatusId
        ];

        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(
                JobEntryStatusIds: jobEntryStatusIds);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Count.ShouldBe(expected: 1);
        jobEntries.First().JobEntryStatusId.ShouldBe(expected: jobEntryStatusIds.First());
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
        jobEntries.Count.ShouldBe(expected: 2);
        jobEntries.First().CurrentRetries.ShouldBeGreaterThanOrEqualTo(expected: moreThanRetries);
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
        jobEntries.Count.ShouldBe(expected: 1);
        jobEntries.First().IsCompleted.ShouldBeTrue();
    }

    [Test]
    public void Should_FilterByHasRun()
    {
        // Arrange
        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(HasRun: true);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Count.ShouldBe(expected: 2);
        jobEntries.First().LastRun.ShouldNotBeNull();
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
        jobEntries.Count.ShouldBe(expected: 1);
        jobEntries.First().Triggers.ShouldNotBeEmpty();
    }

    [Test]
    public void Should_FilterByJobEntryTriggersIds()
    {
        // Arrange
        Guid[] jobEntryTriggersIds = _jobEntries[index: 0].Triggers.Select(selector: x => x.Id).ToArray();

        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification(
                JobEntryTriggersIds: jobEntryTriggersIds);

        // Act
        List<JobEntry> jobEntries =
            _jobEntries.AsQueryable().Where(predicate: _jobEntryQuerySpecification.Filter()).ToList();

        // Assert
        jobEntries.Count.ShouldBe(expected: 1);
        jobEntries.First().Triggers.First().Id.ShouldBe(expected: jobEntryTriggersIds.First());
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
        jobEntries.Count.ShouldBe(expected: 2);
        jobEntries.All(predicate: x => x.Triggers.Count > 0).ShouldBeTrue();
    }
}