using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

using FluentAssertions;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryStatusRepositoryTests;

[TestFixture]
internal sealed class GetManyAsync : JobEntryStatusRepositoryTestBase
{
    [Test]
    public async Task Should_ReturnJobEntryStatuses_When_Exist()
    {
        // Arrange
        // Act
        IReadOnlyCollection<JobEntryStatus> jobEntryStatusCollection =
            await TestCandidate.GetManyAsync(cancellationToken: default);

        // Assert
        jobEntryStatusCollection.Should().NotBeNull();
        jobEntryStatusCollection.Should().HaveCount(expected: JobEntryStatusIds.Count);
    }

    [Test]
    public async Task Should_ReturnEmpty_When_NoneExist()
    {
        // Arrange
        ClearJobEntryStatuses();

        // Act
        IReadOnlyCollection<JobEntryStatus> jobEntryStatusCollection =
            await TestCandidate.GetManyAsync(cancellationToken: default);

        // Assert
        jobEntryStatusCollection.Should().NotBeNull();
        jobEntryStatusCollection.Should().BeEmpty();
    }
}