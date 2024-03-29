using Expenso.Shared.Tests.UnitTests.Queries.TestData;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryDispatcher;

internal sealed class QueryAsync : QueryDispatcherTestBase
{
    [Test]
    public async Task Should_SendQuery()
    {
        // Arrange
        TestQuery testQuery = new(MessageContextFactoryMock.Object.Current(), Guid.NewGuid());

        // Act
        TestResponse? queryResult = await TestCandidate.QueryAsync(testQuery, It.IsAny<CancellationToken>());

        // Assert
        queryResult?.Should().NotBeNull();
        queryResult?.Id.Should().NotBeEmpty();
        queryResult?.Id.Should().Be(testQuery.Id);
        queryResult?.Name.Should().NotBeNullOrEmpty();
        queryResult?.Name.Should().Be("vWdGYZaiMz9cex");
    }
}