using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Proxy.UserPreferencesProxy;

internal sealed class GetUserPreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task Should_ReturnPreferences_When_PreferencesExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(x => x.QueryAsync(
                new GetPreferenceQuery(MessageContextFactoryMock.Object.Current(null), null, _userId, null,
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getPreferenceExternalResponse);

        // Act
        GetPreferenceResponse? preference = await TestCandidate.GetUserPreferencesAsync(_userId, It.IsAny<bool>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>());

        // Assert
        preference.Should().NotBeNull();
        preference.Should().BeEquivalentTo(_getPreferenceExternalResponse);

        _queryDispatcherMock.Verify(
            x => x.QueryAsync(
                new GetPreferenceQuery(MessageContextFactoryMock.Object.Current(null), null, _userId, null,
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_PreferencesDoesNotExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(x => x.QueryAsync(
                new GetPreferenceQuery(MessageContextFactoryMock.Object.Current(null), null, _userId, null,
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetPreferenceResponse?)null);

        // Act
        GetPreferenceResponse? preference = await TestCandidate.GetUserPreferencesAsync(_userId, It.IsAny<bool>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>());

        // Assert
        preference.Should().BeNull();

        _queryDispatcherMock.Verify(
            x => x.QueryAsync(
                new GetPreferenceQuery(MessageContextFactoryMock.Object.Current(null), null, _userId, null,
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), It.IsAny<CancellationToken>()), Times.Once);
    }
}