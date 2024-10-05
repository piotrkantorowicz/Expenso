using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference;

internal sealed class HandleAsync : GetPreferenceQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByIdAndPreferenceExists()
    {
        // Arrange
        GetPreferencesQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(), PreferenceId: _id,
            IncludeFinancePreferences: It.IsAny<bool>(), IncludeNotificationPreferences: It.IsAny<bool>(),
            IncludeGeneralPreferences: It.IsAny<bool>());

        _preferenceRepositoryMock
            .Setup(expression: x => x.GetAsync(
                new PreferenceFilter(_id, null, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        GetPreferenceResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            expression: x => x.GetAsync(
                new PreferenceFilter(_id, null, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ReturnUser_When_SearchingByUserIdAndPreferenceExists()
    {
        // Arrange
        GetPreferencesQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(), UserId: _userId,
            IncludeFinancePreferences: It.IsAny<bool>(), IncludeNotificationPreferences: It.IsAny<bool>(),
            IncludeGeneralPreferences: It.IsAny<bool>());

        _preferenceRepositoryMock
            .Setup(expression: x => x.GetAsync(
                new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        GetPreferenceResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            expression: x => x.GetAsync(
                new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingForCurrentUserAndPreferenceExists()
    {
        // Arrange
        GetPreferencesQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            ForCurrentUser: true,
            IncludeFinancePreferences: It.IsAny<bool>(), IncludeNotificationPreferences: It.IsAny<bool>(),
            IncludeGeneralPreferences: It.IsAny<bool>());

        _userContextAccessorMock.Setup(expression: x => x.Get()).Returns(value: _executionContextMock.Object);

        _preferenceRepositoryMock
            .Setup(expression: x => x.GetAsync(
                new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        GetPreferenceResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            expression: x => x.GetAsync(
                new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()), times: Times.Once);

        _userContextAccessorMock.Verify(expression: x => x.Get(), times: Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingByIdAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferencesQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(), PreferenceId: _id);

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action.Should().ThrowAsync<NotFoundException>().WithMessage(expectedWildcardPattern: "Preferences not found.");
    }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingByUserIdAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferencesQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            PreferenceId: _userId);

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action.Should().ThrowAsync<NotFoundException>().WithMessage(expectedWildcardPattern: "Preferences not found.");
    }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingForCurrentUserAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferencesQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            ForCurrentUser: true);

        // Act
        Func<Task> action = () =>
            TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action.Should().ThrowAsync<NotFoundException>().WithMessage(expectedWildcardPattern: "Preferences not found.");
    }
}