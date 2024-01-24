using Expenso.UserPreferences.Core.Application.Preferences.Internal.Queries.GetPreference;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Proxy.UserPreferencesProxy;

internal sealed class GetUserPreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task Should_ReturnPreferences_When_PreferencesExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(x => x.QueryAsync(new GetPreferenceInternalQuery(_userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getPreferenceInternalResponse);

        // Act
        GetPreferenceInternalResponse? preference =
            await TestCandidate.GetUserPreferencesAsync(_userId, It.IsAny<CancellationToken>());

        // Assert
        preference.Should().NotBeNull();
        preference.Should().BeEquivalentTo(_getPreferenceInternalResponse);

        _queryDispatcherMock.Verify(
            x => x.QueryAsync(new GetPreferenceInternalQuery(_userId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_PreferencesDoesNotExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(x => x.QueryAsync(new GetPreferenceInternalQuery(_userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetPreferenceInternalResponse?)null);

        // Act
        GetPreferenceInternalResponse? preference =
            await TestCandidate.GetUserPreferencesAsync(_userId, It.IsAny<CancellationToken>());

        // Assert
        preference.Should().BeNull();

        _queryDispatcherMock.Verify(
            x => x.QueryAsync(new GetPreferenceInternalQuery(_userId), It.IsAny<CancellationToken>()), Times.Once);
    }
}