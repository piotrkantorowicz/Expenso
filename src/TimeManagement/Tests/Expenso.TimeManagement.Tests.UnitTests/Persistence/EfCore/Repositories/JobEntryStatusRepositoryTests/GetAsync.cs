using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

using FluentAssertions;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryStatusRepositoryTests;

[TestFixture]
internal sealed class GetAsync : JobEntryStatusRepositoryTestBase
{
    [Test]
    public async Task Should_ReturnJobEntryStatus_When_Exists()
    {
        // Arrange
        Guid jobEntryStatusId = JobEntryStatusIds[index: 0];

        // Act
        JobEntryStatus? jobEntryStatus =
            await TestCandidate.GetAsync(id: jobEntryStatusId, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntryStatus.Should().NotBeNull();
        jobEntryStatus?.Id.Should().Be(expected: jobEntryStatusId);
    }

    [Test]
    public async Task Should_ReturnNull_When_NotExists()
    {
        // Arrange
        Guid jobEntryStatusId = Guid.NewGuid();

        // Act
        JobEntryStatus? jobEntryStatus =
            await TestCandidate.GetAsync(id: jobEntryStatusId, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntryStatus.Should().BeNull();
    }
}