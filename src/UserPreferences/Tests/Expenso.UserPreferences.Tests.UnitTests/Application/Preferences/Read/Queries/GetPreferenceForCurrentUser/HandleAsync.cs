using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser;

[TestFixture]
internal sealed class HandleAsync : GetPreferenceForCurrentUserQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnPreference_When_PreferenceExists()
    {
        // Arrange
        GetPreferenceForCurrentUserQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferenceForCurrentUserRequest(
                PreferenceType: It.IsAny<GetPreferenceForCurrentUserRequest_PreferenceTypes>()));

        _userContextAccessorMock.Setup(expression: x => x.Get()).Returns(value: _executionContextMock.Object);

        _preferenceRepositoryMock
            .Setup(expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(null, _userId, false, It.IsAny<PreferenceTypes>()),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        GetPreferenceForCurrentUserResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(null, _userId, false, It.IsAny<PreferenceTypes>()),
                    It.IsAny<CancellationToken>()), times: Times.Once);

        _userContextAccessorMock.Verify(expression: x => x.Get(), times: Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_PreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferenceForCurrentUserQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferenceForCurrentUserRequest(
                PreferenceType: It.IsAny<GetPreferenceForCurrentUserRequest_PreferenceTypes>()));

        // Act
        Func<Task> action = () =>
            TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action.Should().ThrowAsync<NotFoundException>().WithMessage(expectedWildcardPattern: "Preferences not found.");
    }
}