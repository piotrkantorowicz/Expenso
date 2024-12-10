using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories.Specifications;

using FluentAssertions;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryRepository;

[TestFixture]
internal sealed class GetJobEntryAsync : JobEntryRepositoryTestBase
{
    [Test, TestCaseSource(sourceName: nameof(_jobEntriesIds))]
    public async Task Should_ReturnJobEntry_When_JobEntryExists(Guid jobEntryId)
    {
        // Arrange
        JobEntryQuerySpecification querySpecification = new()
        {
            JobEntryId = jobEntryId,
            UseTracking = false
        };

        // Act
        JobEntry? jobEntry = await TestCandidate.GetJobEntryAsync(querySpecification: querySpecification,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntry.Should().NotBeNull();
        jobEntry.Should().Be(expected: JobEntries.Single(predicate: x => x.Id == jobEntryId));
    }
}