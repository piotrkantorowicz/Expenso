using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference;

[TestFixture]
internal sealed class HandleAsync : GetPreferenceQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnPreference_When_SearchingByIdAndPreferenceExists()
    {
        // Arrange
        GetPreferenceQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferenceRequest(PreferenceId: _preferenceId,
                Includes: It.IsAny<GetPreferenceRequestPreferenceIncludes>()));

        _preferenceRepositoryMock
            .Setup(expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(_preferenceId, null, false, It.IsAny<PreferenceIncludes>()),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        GetPreferenceResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expected: _getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(_preferenceId, null, false, It.IsAny<PreferenceIncludes>()),
                    It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_SearchingByIdAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferenceQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferenceRequest(PreferenceId: _preferenceId,
                Includes: It.IsAny<GetPreferenceRequestPreferenceIncludes>()));

        PreferenceQuerySpecification preferenceQuerySpecification = new(PreferenceId: _preferenceId, UseTracking: false,
            Includes: It.IsAny<PreferenceIncludes>());

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();

        exception.Message.ShouldBe(
            expected: $"{nameof(Preference)} with query {preferenceQuerySpecification} hasn't been found.");

        exception.ResourceName.ShouldBe(expected: nameof(Preference));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.Query());

        ((PreferenceQuerySpecification?)exception.Identifier).ShouldBeEquivalentTo(
            expected: preferenceQuerySpecification);
    }
}