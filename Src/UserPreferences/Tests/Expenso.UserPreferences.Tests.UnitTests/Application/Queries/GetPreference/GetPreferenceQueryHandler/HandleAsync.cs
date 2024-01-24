using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Queries.GetPreference;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Queries.GetPreference.GetPreferenceQueryHandler;

internal sealed class HandleAsync : GetPreferenceQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByIdAndPreferenceExists()
    {
        // Arrange
        GetPreferenceQuery query = new(_id);

        _preferenceRepositoryMock
            .Setup(x => x.GetByIdAsync(_id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        GetPreferenceResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getPreferenceResponse);

        _preferenceRepositoryMock.Verify(x => x.GetByIdAsync(_id, It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Should_ReturnUser_When_SearchingByUserIdAndPreferenceExists()
    {
        // Arrange
        GetPreferenceQuery query = new(UserId: _userId);

        _preferenceRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        GetPreferenceResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingForCurrentUserAndPreferenceExists()
    {
        // Arrange
        GetPreferenceQuery query = new();
        _userContextAccessorMock.Setup(x => x.Get()).Returns(_userContextMock.Object);

        _preferenceRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        GetPreferenceResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);

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

        string expectedExceptionMessage = new StringBuilder()
            .Append("Preferences with id ")
            .Append(_id)
            .Append(" not found.")
            .ToString();

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

        string expectedExceptionMessage = new StringBuilder()
            .Append("Preferences for user with id ")
            .Append(_userId)
            .Append(" not found.")
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_SearchingForCurrentUserAndPreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferenceQuery query = new();

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.HandleAsync(query));

        const string expectedExceptionMessage =
            "Preferences for current user not found, because user id from user context is empty.";

        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}