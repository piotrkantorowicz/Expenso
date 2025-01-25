using Expenso.Shared.Tests.UnitTests.Queries.TestData;

using Moq;

using NUnit.Framework;

using Shouldly;

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
        queryResult?.ShouldNotBeNull();
        queryResult?.Id.ShouldNotBe(expected: Guid.Empty);
        queryResult?.Id.ShouldBe(expected: _testQuery.Id);
        queryResult?.Name.ShouldNotBeEmpty();
        queryResult?.Name.ShouldBe(expected: "vWdGYZaiMz9cex");
    }

    [Test]
    public void Should_TrackMessageContext()
    {
        // Arrange
        // Act
        // Assert
        _testQuery.MessageContext.ShouldNotBeNull();

        _testQuery.MessageContext.CorrelationId.ShouldBe(expected: MessageContextFactoryMock.Object.Current()
            .CorrelationId);

        _testQuery.MessageContext.MessageId.ShouldBe(expected: MessageContextFactoryMock.Object.Current().MessageId);

        _testQuery.MessageContext.RequestedBy.ShouldBe(expected: MessageContextFactoryMock.Object.Current()
            .RequestedBy);

        _testQuery.MessageContext.Timestamp.ShouldBe(expected: MessageContextFactoryMock.Object.Current().Timestamp);
        _testQuery.MessageContext.ModuleId.ShouldBe(expected: MessageContextFactoryMock.Object.Current().ModuleId);
    }
}