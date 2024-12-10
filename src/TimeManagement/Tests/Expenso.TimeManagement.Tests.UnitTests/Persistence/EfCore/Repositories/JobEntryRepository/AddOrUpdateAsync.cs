using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

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
            Id = Guid.NewGuid()
        };

        _jobEntriesDbSetMock
            .Setup(expression: x => x.AddAsync(jobEntry, It.IsAny<CancellationToken>()))
            .Callback<JobEntry, CancellationToken>(action: (entity, _) => AddJobEntry(jobEntry: entity));

        // Act
        await TestCandidate.AddOrUpdateAsync(jobEntry: jobEntry, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _jobEntriesDbSetMock.Object.Should().Contain(expected: jobEntry);
    }

    [Test]
    public async Task Should_UpdateJobEntry_When_JobEntryHasExists()
    {
        // Arrange
        JobEntry dbJobEntry = JobEntries[index: 0];
        dbJobEntry.CurrentRetries = 5;
        dbJobEntry.IsCompleted = false;
        _dbContextMock.Setup(expression: x => x.GetEntryState(dbJobEntry)).Returns(value: EntityState.Modified);

        // Act
        await TestCandidate.AddOrUpdateAsync(jobEntry: dbJobEntry, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _jobEntriesDbSetMock.Object.Should().Contain(expected: dbJobEntry);
    }
}