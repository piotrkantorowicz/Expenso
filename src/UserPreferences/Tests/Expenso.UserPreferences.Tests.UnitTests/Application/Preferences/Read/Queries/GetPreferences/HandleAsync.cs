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
}