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
            .Setup(expression: x => x.QueryAsync(new GetPreferenceQuery(MessageContextFactoryMock.Object.Current(null),
                _userId, null,
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getPreferenceExternalResponse);

        // Act
        GetPreferenceResponse? preference = await TestCandidate.GetUserPreferencesAsync(id: _userId,
            includeFinancePreferences: It.IsAny<bool>(), includeNotificationPreferences: It.IsAny<bool>(),
            includeGeneralPreferences: It.IsAny<bool>(), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        preference.Should().NotBeNull();
        preference.Should().BeEquivalentTo(expectation: _getPreferenceExternalResponse);

        _queryDispatcherMock.Verify(
            expression: x => x.QueryAsync(new GetPreferenceQuery(MessageContextFactoryMock.Object.Current(null),
                _userId, null,
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_PreferencesDoesNotExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(new GetPreferenceQuery(MessageContextFactoryMock.Object.Current(null),
                _userId, null,
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        GetPreferenceResponse? preference = await TestCandidate.GetUserPreferencesAsync(id: _userId,
            includeFinancePreferences: It.IsAny<bool>(), includeNotificationPreferences: It.IsAny<bool>(),
            includeGeneralPreferences: It.IsAny<bool>(), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        preference.Should().BeNull();

        _queryDispatcherMock.Verify(
            expression: x => x.QueryAsync(new GetPreferenceQuery(MessageContextFactoryMock.Object.Current(null),
                _userId, null,
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }
}