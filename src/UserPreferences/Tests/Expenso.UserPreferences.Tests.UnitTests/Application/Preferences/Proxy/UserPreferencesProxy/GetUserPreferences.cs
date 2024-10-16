using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Proxy.UserPreferencesProxy;

[TestFixture]
internal sealed class GetUserPreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task Should_ReturnPreferences_When_PreferencesExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x =>
                x.QueryAsync(
                    new GetPreferencesQuery(MessageContextFactoryMock.Object.Current(null),
                        new GetPreferencesRequest(null, _userId, It.IsAny<GetPreferencesRequest_PreferenceTypes>())),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getPreferencesExternalResponse);

        // Act
        GetPreferencesResponse? preference = await TestCandidate.GetPreferences(
            getPreferenceRequest: new GetPreferencesRequest(PreferenceId: null, UserId: _userId,
                PreferenceType: It.IsAny<GetPreferencesRequest_PreferenceTypes>()),
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        preference.Should().NotBeNull();
        preference.Should().BeEquivalentTo(expectation: _getPreferencesExternalResponse);

        _queryDispatcherMock.Verify(
            expression: x =>
                x.QueryAsync(
                    new GetPreferencesQuery(MessageContextFactoryMock.Object.Current(null),
                        new GetPreferencesRequest(null, _userId, It.IsAny<GetPreferencesRequest_PreferenceTypes>())),
                    It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_PreferencesDoesNotExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x =>
                x.QueryAsync(
                    new GetPreferencesQuery(MessageContextFactoryMock.Object.Current(null),
                        new GetPreferencesRequest(null, _userId, It.IsAny<GetPreferencesRequest_PreferenceTypes>())),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        GetPreferencesResponse? preference = await TestCandidate.GetPreferences(
            getPreferenceRequest: new GetPreferencesRequest(PreferenceId: null, UserId: _userId,
                PreferenceType: It.IsAny<GetPreferencesRequest_PreferenceTypes>()),
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        preference.Should().BeNull();

        _queryDispatcherMock.Verify(
            expression: x =>
                x.QueryAsync(
                    new GetPreferencesQuery(MessageContextFactoryMock.Object.Current(null),
                        new GetPreferencesRequest(null, _userId, It.IsAny<GetPreferencesRequest_PreferenceTypes>())),
                    It.IsAny<CancellationToken>()), times: Times.Once);
    }
}