using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using FluentAssertions;

using Moq;

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
            await TestCandidate.GetManyAsync(cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntryStatusCollection.Should().NotBeNull();
        jobEntryStatusCollection.Should().HaveCount(expected: JobEntryStatusIds.Count);
    }

    [Test]
    public async Task Should_ReturnEmpty_When_NoneExist()
    {
        // Arrange
        _jobEntryStatuses.Clear();

        // Act
        IReadOnlyCollection<JobEntryStatus> jobEntryStatusCollection =
            await TestCandidate.GetManyAsync(cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntryStatusCollection.Should().NotBeNull();
        jobEntryStatusCollection.Should().BeEmpty();
    }
}