using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference;

[TestFixture]
internal sealed class HandleAsync : GetPreferenceQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnPreference_When_SearchingByIdAndPreferenceExists()
    {
        // Arrange
        GetPreferenceQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferenceRequest(PreferenceId: _id,
                PreferenceType: It.IsAny<GetPreferenceRequest_PreferenceTypes>()));

        _preferenceRepositoryMock
            .Setup(expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(_id, null, false, It.IsAny<PreferenceTypes>()),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        GetPreferenceResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getPreferenceResponse);

        _preferenceRepositoryMock.Verify(expression: x =>
            x.GetAsync(new PreferenceQuerySpecification(_id, null, false, It.IsAny<PreferenceTypes>()),
                It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingByIdAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferenceQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferenceRequest(PreferenceId: _id,
                PreferenceType: It.IsAny<GetPreferenceRequest_PreferenceTypes>()));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action.Should().ThrowAsync<NotFoundException>().WithMessage(expectedWildcardPattern: "Preferences not found.");
    }
}