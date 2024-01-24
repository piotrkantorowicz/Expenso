using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Internal.Queries.GetPreference;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Internal.Queries.GetPreference.
    GetPreferenceInternalQueryHandler;

internal sealed class HandleAsync : GetPreferenceInternalQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByUserIdAndPreferenceExists()
    {
        // Arrange
        GetPreferenceInternalQuery query = new(_userId);

        _preferenceRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        GetPreferenceInternalResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingByUserIdAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferenceInternalQuery query = new(_userId);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.HandleAsync(query));

        string expectedExceptionMessage = new StringBuilder()
            .Append("Preferences for user with id ")
            .Append(_userId)
            .Append(" not found.")
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}