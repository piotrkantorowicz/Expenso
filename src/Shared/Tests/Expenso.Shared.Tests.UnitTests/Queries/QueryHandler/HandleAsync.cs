using Expenso.Shared.Tests.UnitTests.Queries.TestData;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandler;

[TestFixture]
internal sealed class HandleAsync : QueryHandlerResultTestBase
{
    [Test]
    public async Task Should_HandleQuery()
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

    [Test]
    public void Should_TrackMessageContext()
    {
        // Arrange
        // Act
        // Assert
        _testQuery.MessageContext.Should().NotBeNull();

        _testQuery
            .MessageContext.CorrelationId.Should()
            .Be(expected: MessageContextFactoryMock.Object.Current().CorrelationId);

        _testQuery.MessageContext.MessageId.Should().Be(expected: MessageContextFactoryMock.Object.Current().MessageId);

        _testQuery
            .MessageContext.RequestedBy.Should()
            .Be(expected: MessageContextFactoryMock.Object.Current().RequestedBy);

        _testQuery.MessageContext.Timestamp.Should().Be(expected: MessageContextFactoryMock.Object.Current().Timestamp);
        _testQuery.MessageContext.ModuleId.Should().Be(expected: MessageContextFactoryMock.Object.Current().ModuleId);
    }
}