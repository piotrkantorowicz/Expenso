using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryRepository;

[TestFixture]
internal sealed class GetJobEntryAsync : JobEntryRepositoryTestBase
{
    [Test, TestCaseSource(sourceName: nameof(_jobEntriesIds))]
    public async Task Should_ReturnJobEntry_When_JobEntryExists(Guid jobEntryId)
    {
        // Arrange
        JobEntryQuerySpecification querySpecification = new(JobEntryId: jobEntryId, UseTracking: false);

        // Act
        JobEntry? jobEntry = await TestCandidate.GetJobEntryAsync(querySpecification: querySpecification,
            cancellationToken: default);

        // Assert
        jobEntry.Should().NotBeNull();
        jobEntry.Should().Be(expected: JobEntries.Single(predicate: x => x.Id == jobEntryId));
    }

    [Test]
    public async Task Should_ReturnNull_When_JobEntryDoesNotExist()
    {
        // Arrange
        Guid nonExistentId = Guid.NewGuid();
        JobEntryQuerySpecification querySpecification = new(JobEntryId: nonExistentId, UseTracking: false);

        // Act
        JobEntry? jobEntry = await TestCandidate.GetJobEntryAsync(querySpecification: querySpecification,
            cancellationToken: default);

        // Assert
        jobEntry.Should().BeNull();
    }
}