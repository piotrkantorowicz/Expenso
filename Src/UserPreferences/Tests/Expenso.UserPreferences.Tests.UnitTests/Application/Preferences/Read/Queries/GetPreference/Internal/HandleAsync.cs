using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference.Internal;

internal sealed class HandleAsync : GetPreferenceQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByIdAndPreferenceExists()
    {
        // Arrange
        GetPreferenceQuery query = new(_id, IncludeFinancePreferences: It.IsAny<bool>(),
            IncludeNotificationPreferences: It.IsAny<bool>(), IncludeGeneralPreferences: It.IsAny<bool>());

        _preferenceRepositoryMock
            .Setup(x => x.GetAsync(
                new PreferenceFilter(_id, null, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        GetPreferenceResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            x => x.GetAsync(
                new PreferenceFilter(_id, null, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Should_ReturnUser_When_SearchingByUserIdAndPreferenceExists()
    {
        // Arrange
        GetPreferenceQuery query = new(UserId: _userId, IncludeFinancePreferences: It.IsAny<bool>(),
            IncludeNotificationPreferences: It.IsAny<bool>(), IncludeGeneralPreferences: It.IsAny<bool>());

        _preferenceRepositoryMock
            .Setup(x => x.GetAsync(
                new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        GetPreferenceResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            x => x.GetAsync(
                new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingForCurrentUserAndPreferenceExists()
    {
        // Arrange
        GetPreferenceQuery query = new(ForCurrentUser: true, IncludeFinancePreferences: It.IsAny<bool>(),
            IncludeNotificationPreferences: It.IsAny<bool>(), IncludeGeneralPreferences: It.IsAny<bool>());

        _userContextAccessorMock.Setup(x => x.Get()).Returns(_userContextMock.Object);

        _preferenceRepositoryMock
            .Setup(x => x.GetAsync(
                new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        GetPreferenceResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            x => x.GetAsync(
                new PreferenceFilter(null, _userId, false, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                It.IsAny<CancellationToken>()), Times.Once);

        _userContextAccessorMock.Verify(x => x.Get(), Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingByIdAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferenceQuery query = new(_id);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.HandleAsync(query));
        const string expectedExceptionMessage = "Preferences not found";
        exception?.Message.Should().Be(expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingByUserIdAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferenceQuery query = new(UserId: _userId);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.HandleAsync(query));
        const string expectedExceptionMessage = "Preferences not found";
        exception?.Message.Should().Be(expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingForCurrentUserAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferenceQuery query = new(ForCurrentUser: true);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.HandleAsync(query));
        const string expectedExceptionMessage = "Preferences not found";
        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}