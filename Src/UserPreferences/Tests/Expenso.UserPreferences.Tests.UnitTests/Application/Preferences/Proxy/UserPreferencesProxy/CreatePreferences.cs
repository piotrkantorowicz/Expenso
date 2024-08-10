using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Proxy.UserPreferencesProxy;

internal sealed class CreatePreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task Should_CallCreatePreference()
    {
        // Arrange
        CreatePreferenceRequest createPreferenceRequest = new(UserId: _userId);

        _commandDispatcherMock
            .Setup(expression: x => x.SendAsync<CreatePreferenceCommand, CreatePreferenceResponse>(
                new CreatePreferenceCommand(MessageContextFactoryMock.Object.Current(null), createPreferenceRequest),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _createPreferenceResponse);

        // Act
        CreatePreferenceResponse? preference = await TestCandidate.CreatePreferencesAsync(
            createPreferenceRequest: createPreferenceRequest, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        preference.Should().NotBeNull();
        preference.Should().BeEquivalentTo(expectation: _createPreferenceResponse);

        _commandDispatcherMock.Verify(
            expression: x => x.SendAsync<CreatePreferenceCommand, CreatePreferenceResponse>(
                new CreatePreferenceCommand(MessageContextFactoryMock.Object.Current(null),
                    new CreatePreferenceRequest(_userId)), It.IsAny<CancellationToken>()), times: Times.Once);
    }
}