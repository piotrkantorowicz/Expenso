using Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Proxy.UserPreferencesProxy;

internal sealed class CreatePreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task Should_CallCreatePreference()
    {
        // Arrange
        _commandDispatcherMock
            .Setup(x => x.SendAsync<CreatePreferenceInternalCommand, CreatePreferenceInternalResponse>(
                new CreatePreferenceInternalCommand(new CreatePreferenceInternalRequest(_userId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_createPreferenceInternalResponse);

        // Act
        CreatePreferenceInternalResponse? preference =
            await TestCandidate.CreatePreferencesAsync(_userId, It.IsAny<CancellationToken>());

        // Assert
        preference.Should().NotBeNull();
        preference.Should().BeEquivalentTo(_createPreferenceInternalResponse);

        _commandDispatcherMock.Verify(
            x => x.SendAsync<CreatePreferenceInternalCommand, CreatePreferenceInternalResponse>(
                new CreatePreferenceInternalCommand(new CreatePreferenceInternalRequest(_userId)),
                It.IsAny<CancellationToken>()), Times.Once);
    }
}