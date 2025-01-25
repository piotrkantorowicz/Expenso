using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

using NUnit.Framework;

using Shouldly;

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
        jobEntryStatus.ShouldNotBeNull();
        jobEntryStatus?.Id.ShouldBe(expected: expectedJobEntryStatus.Id);
        jobEntryStatus?.Name.ShouldBe(expected: expectedJobEntryStatus.Name);
        jobEntryStatus?.Description.ShouldBe(expected: expectedJobEntryStatus.Description);
    }

    [Test]
    [TestCase(arg: "00000000-0000-0000-0000-000000000000", Description = "Empty GUID")]
    [TestCase(arg: "A5C2C3D4-E5F6-47A8-B9C0-1D2E3F4A5B6C", Description = "Non-existent GUID")]
    public async Task Should_ReturnNull_WhenStatusDoesNotExist(string id)
    {
        // Arrange
        Guid jobEntryStatusId = Guid.Parse(input: id);

        // Act
        JobEntryStatus? jobEntryStatus = await TestCandidate.GetAsync(id: jobEntryStatusId, cancellationToken: default);

        // Assert
        jobEntryStatus.ShouldBeNull();
    }
}