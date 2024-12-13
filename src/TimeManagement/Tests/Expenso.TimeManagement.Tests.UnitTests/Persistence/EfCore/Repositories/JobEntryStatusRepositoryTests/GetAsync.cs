using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

using FluentAssertions;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryStatusRepositoryTests;

[TestFixture]
internal sealed class GetAsync : JobEntryStatusRepositoryTestBase
{
    [Test]
    [TestCase(arg: 0)]
    [TestCase(arg: 1)]
    [TestCase(arg: 2)]
    public async Task Should_ReturnJobEntryStatus_When_Exists(int index)
    {
        // Arrange
        JobEntryStatus expectedJobEntryStatus = _jobEntryStatuses[index: index];

        // Act
        JobEntryStatus? jobEntryStatus =
            await TestCandidate.GetAsync(id: expectedJobEntryStatus.Id, cancellationToken: default);

        // Assert
        jobEntryStatus.Should().NotBeNull();
        jobEntryStatus?.Id.Should().Be(expected: expectedJobEntryStatus.Id);
        jobEntryStatus?.Name.Should().Be(expected: expectedJobEntryStatus.Name);
        jobEntryStatus?.Description.Should().Be(expected: expectedJobEntryStatus.Description);
    }

    [Test]
    [TestCase(arg: "00000000-0000-0000-0000-000000000000", Description = "Empty GUID")]
    [TestCase(arg: "A5C2C3D4-E5F6-47A8-B9C0-1D2E3F4A5B6C", Description = "Non-existent GUID")]
    public async Task GetAsync_ShouldReturnNull_WhenStatusDoesNotExist(string id)
    {
        // Arrange
        Guid jobEntryStatusId = Guid.NewGuid();

        // Act
        JobEntryStatus? jobEntryStatus = await TestCandidate.GetAsync(id: jobEntryStatusId, cancellationToken: default);

        // Assert
        jobEntryStatus.Should().BeNull();
    }
}