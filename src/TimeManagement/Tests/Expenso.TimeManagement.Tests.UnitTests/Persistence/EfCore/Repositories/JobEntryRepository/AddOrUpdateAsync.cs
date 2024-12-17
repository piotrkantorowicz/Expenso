using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryRepository;

[TestFixture]
internal sealed class AddOrUpdateAsync : JobEntryRepositoryTestBase
{
    [Test]
    public async Task Should_AddJobEntry_When_JobEntryHasNotExists()
    {
        // Arrange
        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            CurrentRetries = 0,
            MaxRetries = 3,
            IsCompleted = false,
            RunAt = DateTime.UtcNow.AddMinutes(value: 30)
        };

        _jobEntriesDbSetMock
            .Setup(expression: x => x.AddAsync(jobEntry, default))
            .Callback<JobEntry, CancellationToken>(action: (entity, _) => AddJobEntry(jobEntry: entity));

        // Act
        await TestCandidate.AddOrUpdateAsync(jobEntry: jobEntry, cancellationToken: default);

        // Assert
        _jobEntriesDbSetMock.Object.Should().Contain(expected: jobEntry);
    }

    [Test]
    public async Task Should_UpdateJobEntry_When_JobEntryHasExists()
    {
        // Arrange
        const int currentRetries = 5;
        const bool isCompleted = false;
        JobEntry dbJobEntry = JobEntries[index: 0];
        dbJobEntry.CurrentRetries = currentRetries;
        dbJobEntry.IsCompleted = isCompleted;
        _dbContextMock.Setup(expression: x => x.GetEntryState(dbJobEntry)).Returns(value: EntityState.Modified);

        // Act
        await TestCandidate.AddOrUpdateAsync(jobEntry: dbJobEntry, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        dbJobEntry.CurrentRetries.Should().Be(expected: currentRetries);
        dbJobEntry.IsCompleted.Should().Be(expected: isCompleted);
    }
}