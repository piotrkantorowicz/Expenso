using Expenso.Shared.Tests.UnitTests.Queries.TestData;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandler;

internal sealed class HandleAsync : QueryHandlerResultTestBase
{
    [Test]
    public async Task Should_HandleCommand()
    {
        // Arrange
        // Act
        TestResponse? queryResult =
            await TestCandidate.HandleAsync(query: _testQuery, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        queryResult?.Should().NotBeNull();
        queryResult?.Id.Should().NotBeEmpty();
        queryResult?.Id.Should().Be(expected: _testQuery.Id);
        queryResult?.Name.Should().NotBeNullOrEmpty();
        queryResult?.Name.Should().Be(expected: "vWdGYZaiMz9cex");
    }
}