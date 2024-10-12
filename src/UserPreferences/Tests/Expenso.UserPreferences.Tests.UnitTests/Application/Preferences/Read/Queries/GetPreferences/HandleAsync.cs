using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreferences;

internal sealed class HandleAsync : GetPreferencesQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnPreference_When_SearchingByIdAndPreferenceExists()
    {
        // Arrange
        GetPreferencesQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferencesRequest(PreferenceId: _id,
                PreferenceType: It.IsAny<GetPreferencesRequest_PreferenceTypes>()));

        _preferenceRepositoryMock
            .Setup(expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(_id, null, false, It.IsAny<PreferenceTypes>()),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        GetPreferencesResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            expression: x => x.GetAsync(new PreferenceQuerySpecification(_id, null, false, It.IsAny<PreferenceTypes>()),
                It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ReturnPreference_When_SearchingByUserIdAndPreferenceExists()
    {
        // Arrange
        GetPreferencesQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferencesRequest(UserId: _userId,
                PreferenceType: It.IsAny<GetPreferencesRequest_PreferenceTypes>()));

        _preferenceRepositoryMock
            .Setup(expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(null, _userId, false, It.IsAny<PreferenceTypes>()),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        GetPreferencesResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(null, _userId, false, It.IsAny<PreferenceTypes>()),
                    It.IsAny<CancellationToken>()), times: Times.Once);
    }

    // [Test]
    // public async Task Should_ReturnNull_When_SearchingForCurrentUserAndPreferenceExists()
    // {
    //     // Arrange
    //     GetPreferenceQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(), ForCurrentUser: true,
    //         IncludeFinancePreferences: It.IsAny<bool>(), IncludeNotificationPreferences: It.IsAny<bool>(),
    //         IncludeGeneralPreferences: It.IsAny<bool>());
    //
    //     _userContextAccessorMock.Setup(expression: x => x.Get()).Returns(value: _executionContextMock.Object);
    //
    //     _preferenceRepositoryMock
    //         .Setup(expression: x => x.GetAsync(
    //             new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
    //             It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(value: _preference);
    //
    //     // Act
    //     GetPreferenceResponse? result =
    //         await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());
    //
    //     // Assert
    //     result.Should().NotBeNull();
    //     result.Should().BeEquivalentTo(expectation: _getPreferenceResponse);
    //
    //     _preferenceRepositoryMock.Verify(
    //         expression: x => x.GetAsync(
    //             new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
    //             It.IsAny<CancellationToken>()), times: Times.Once);
    //
    //     _userContextAccessorMock.Verify(expression: x => x.Get(), times: Times.Once);
    // }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingByIdAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferencesQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferencesRequest(PreferenceId: _id,
                PreferenceType: It.IsAny<GetPreferencesRequest_PreferenceTypes>()));

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
            Payload: new GetPreferencesRequest(UserId: _userId,
                PreferenceType: It.IsAny<GetPreferencesRequest_PreferenceTypes>()));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action.Should().ThrowAsync<NotFoundException>().WithMessage(expectedWildcardPattern: "Preferences not found.");
    }

    // [Test]
    // public void Should_ThrowNotFoundException_When_SearchingForCurrentUserAndPreferenceHasNotBeenFound()
    // {
    //     // Arrange
    //     GetPreferenceQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
    //         ForCurrentUser: true);
    //
    //     // Act
    //     Func<Task> action = () =>
    //         TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());
    //
    //     // Assert
    //     action.Should().ThrowAsync<NotFoundException>().WithMessage(expectedWildcardPattern: "Preferences not found.");
    // }
}