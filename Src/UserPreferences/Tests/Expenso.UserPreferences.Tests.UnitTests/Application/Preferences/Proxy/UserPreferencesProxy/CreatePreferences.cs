using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.External;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Proxy.UserPreferencesProxy;

internal sealed class CreatePreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task Should_CallCreatePreference()
    {
        // Arrange
        _commandDispatcherMock
            .Setup(x => x.SendAsync<CreatePreferenceCommand, CreatePreferenceResponse>(
                new CreatePreferenceCommand(new CreatePreferenceRequest(_userId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_createPreferenceResponse);

        // Act
        CreatePreferenceResponse? preference =
            await TestCandidate.CreatePreferencesAsync(_userId, It.IsAny<CancellationToken>());

        // Assert
        preference.Should().NotBeNull();
        preference.Should().BeEquivalentTo(_createPreferenceResponse);

        _commandDispatcherMock.Verify(
            x => x.SendAsync<CreatePreferenceCommand, CreatePreferenceResponse>(
                new CreatePreferenceCommand(new CreatePreferenceRequest(_userId)), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}