using System.Text;

using Expenso.IAM.Core.Users.Internal.Queries.GetUser;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Internal.Queries.GetUser.Cases;

internal sealed class HandleAsync : GetUserQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByIdAndUserExists()
    {
        // Arrange
        GetUserQuery query = new(_userId);
        _userServiceMock.Setup(x => x.GetUserByIdInternalAsync(_userId)).ReturnsAsync(_getUserInternalResponse);

        // Act
        GetUserInternalResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getUserInternalResponse);
    }

    [Test]
    public async Task Should_ReturnUser_When_SearchingByEmailAndUserExists()
    {
        // Arrange
        GetUserQuery query = new(Email: _userEmail);
        _userServiceMock.Setup(x => x.GetUserByEmailInternalAsync(_userEmail)).ReturnsAsync(_getUserInternalResponse);

        // Act
        GetUserInternalResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getUserInternalResponse);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByIdAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserQuery query = new(_userId);
        _userServiceMock.Setup(x => x.GetUserByIdInternalAsync(_userId))!.ReturnsAsync((GetUserInternalResponse?)null);

        // Act
        GetUserInternalResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByEmailAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserQuery query = new(Email: _userEmail);

        _userServiceMock.Setup(x => x.GetUserByEmailInternalAsync(_userEmail))!.ReturnsAsync(
            (GetUserInternalResponse?)null);

        // Act
        GetUserInternalResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ThrowNotFoundException_When_QueryIsEmpty()
    {
        // Arrange
        GetUserQuery query = new();

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.HandleAsync(query));

        string expectedExceptionMessage = new StringBuilder()
            .Append(nameof(query.Id))
            .Append(" or ")
            .Append(nameof(query.Email))
            .Append(" must be provided.")
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}