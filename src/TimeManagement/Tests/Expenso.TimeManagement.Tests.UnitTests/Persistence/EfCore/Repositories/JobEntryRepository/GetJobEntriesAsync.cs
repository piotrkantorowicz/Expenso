using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryRepository;

[TestFixture]
internal sealed class GetJobEntriesAsync : JobEntryRepositoryTestBase
{
    [Test]
    public async Task Should_ReturnJobEntries_When_JobEntryExists()
    {
        // Arrange
        JobEntryQuerySpecification querySpecification = new(UseTracking: false);

        // Act
        IReadOnlyCollection<JobEntry> jobEntries =
            await TestCandidate.GetJobEntriesAsync(querySpecification: querySpecification, cancellationToken: default);

        // Assert
        jobEntries.Should().NotBeNull();
        jobEntries.Should().HaveCount(expected: _jobEntriesIds.Count);
    }
}