using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Proxy.UserPreferencesProxy;

[TestFixture]
internal sealed class GetUserPreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task Should_ReturnPreferences_When_PreferencesExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(new GetPreferencesQuery(
                    MessageContextFactoryMock.Object.FromParent(_currentMessageContext, It.IsAny<string?>(),
                        It.IsAny<Guid>()),
                    new GetPreferencesRequest(null, _userId, It.IsAny<GetPreferencesRequestPreferenceIncludes>())),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getPreferencesExternalResponse);

        // Act
        GetPreferencesResponse? preference = await TestCandidate.GetPreferences(
            getPreferenceRequest: new GetPreferencesRequest(PreferenceId: null, UserId: _userId,
                Includes: It.IsAny<GetPreferencesRequestPreferenceIncludes>()),
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        preference.ShouldNotBeNull();
        preference.ShouldBeEquivalentTo(expected: _getPreferencesExternalResponse);

        _queryDispatcherMock.Verify(expression: x => x.QueryAsync(new GetPreferencesQuery(
                MessageContextFactoryMock.Object.FromParent(_currentMessageContext, It.IsAny<string?>(),
                    It.IsAny<Guid>()),
                new GetPreferencesRequest(null, _userId, It.IsAny<GetPreferencesRequestPreferenceIncludes>())),
                    It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_PreferencesDoesNotExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(new GetPreferencesQuery(
                    MessageContextFactoryMock.Object.FromParent(_currentMessageContext, It.IsAny<string?>(),
                        It.IsAny<Guid>()),
                    new GetPreferencesRequest(null, _userId, It.IsAny<GetPreferencesRequestPreferenceIncludes>())),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        GetPreferencesResponse? preference = await TestCandidate.GetPreferences(
            getPreferenceRequest: new GetPreferencesRequest(PreferenceId: null, UserId: _userId,
                Includes: It.IsAny<GetPreferencesRequestPreferenceIncludes>()),
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        preference.ShouldBeNull();

        _queryDispatcherMock.Verify(expression: x => x.QueryAsync(new GetPreferencesQuery(
                MessageContextFactoryMock.Object.FromParent(_currentMessageContext, It.IsAny<string?>(),
                    It.IsAny<Guid>()),
                new GetPreferencesRequest(null, _userId, It.IsAny<GetPreferencesRequestPreferenceIncludes>())),
                    It.IsAny<CancellationToken>()), times: Times.Once);
    }
}