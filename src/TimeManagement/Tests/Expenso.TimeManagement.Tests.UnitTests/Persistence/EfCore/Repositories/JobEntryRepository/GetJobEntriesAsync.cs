using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories.Specifications;

using FluentAssertions;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryRepository;

[TestFixture]
internal sealed class GetJobEntriesAsync : JobEntryRepositoryTestBase
{
    [Test]
    public async Task Should_ReturnJobEntries_When_JobEntryExistsExists()
    {
        // Arrange
        JobEntryQuerySpecification querySpecification = new()
        {
            UseTracking = false
        };

        // Act
        IReadOnlyCollection<JobEntry> jobEntries =
            await TestCandidate.GetJobEntriesAsync(querySpecification: querySpecification,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntries.Should().NotBeNull();
        jobEntries.Should().HaveCount(expected: _jobEntriesIds.Count);
    }
}