using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

using NUnit.Framework;

using Shouldly;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobInstanceRepository;

[TestFixture]
internal sealed class GetAsync : JobInstanceRepositoryTestBase
{
    [Test]
    public async Task Should_ReturnJobInstance_When_Exists()
    {
        // Arrange
        Guid jobInstanceId = JobInstanceIds[index: 0];

        // Act
        JobInstance? jobInstance = await TestCandidate.GetAsync(id: jobInstanceId, cancellationToken: default);

        // Assert
        jobInstance.ShouldNotBeNull();
        jobInstance?.Id.ShouldBe(expected: jobInstanceId);
    }

    [Test]
    public async Task Should_ReturnNull_When_NotExists()
    {
        // Arrange
        Guid jobInstanceId = Guid.NewGuid();

        // Act
        JobInstance? jobInstance = await TestCandidate.GetAsync(id: jobInstanceId, cancellationToken: default);

        // Assert
        jobInstance.ShouldBeNull();
    }
}