using Expenso.Shared.Tests.UnitTests.Queries.TestData;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryDispatcher;

[TestFixture]
internal sealed class QueryAsync : QueryDispatcherTestBase
{
    [Test]
    public async Task Should_SendQuery()
    {
        // Arrange
        TestQuery testQuery = new(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid());

        // Act
        TestResponse? queryResult =
            await TestCandidate.QueryAsync(query: testQuery, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        queryResult?.ShouldNotBeNull();
        queryResult?.Id.ShouldNotBe(expected: Guid.Empty);
        queryResult?.Id.ShouldBe(expected: testQuery.Id);
        queryResult?.Name.ShouldNotBeEmpty();
        queryResult?.Name.ShouldBe(expected: "vWdGYZaiMz9cex");
    }
}