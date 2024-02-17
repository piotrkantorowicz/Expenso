using System.Text;

using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.External;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference.External;

internal sealed class HandleAsync : GetPreferenceQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByUserIdAndPreferenceExists()
    {
        // Arrange
        GetPreferenceQuery query = new(_userId, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>());

        _preferenceRepositoryMock
            .Setup(x => x.GetAsync(
                new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        GetPreferenceResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            x => x.GetAsync(
                new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingByUserIdAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferenceQuery query = new(_userId);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.HandleAsync(query));

        string expectedExceptionMessage = new StringBuilder()
            .Append("Preferences for user with id ")
            .Append(_userId)
            .Append(" not found")
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}