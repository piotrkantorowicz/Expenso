using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

[TestFixture]
internal sealed class CreatePreferences : PreferencesTestBase
{
    private static IEnumerable<object> InvalidUserIdCases()
    {
        yield return new TestCaseData(args: null).SetName(name: "Should_Return422_When_UserIdIsNull");
        yield return new TestCaseData(arg: Guid.Empty).SetName(name: "Should_Return422_When_UserIdIsEmpty");
    }

    [Test]
    public async Task Should_Return201_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid userId = Guid.CreateVersion7();
        Guid preferenceId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new CreatePreferenceRequest(PreferenceId: preferenceId, UserId: userId));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.Created);

        CreatePreferenceResponse? responseContent =
            await response.Content.ReadFromJsonAsync<CreatePreferenceResponse>();

        responseContent?.ShouldNotBeNull();
        responseContent?.PreferenceId.ShouldBe(expected: preferenceId);
    }

    [Test]
    public async Task Should_Return409_When_ResourceWithProvidedPreferenceIdAlreadyExists()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid userId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new CreatePreferenceRequest(PreferenceId: PreferencesDataInitializer.PreferenceIds[index: 0],
                UserId: userId));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.Conflict);
    }

    [Test, TestCaseSource(sourceName: nameof(InvalidUserIdCases))]
    public async Task Should_HandleInvalidRequest(Guid userId)
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new CreatePreferenceRequest(PreferenceId: Guid.NewGuid(), UserId: userId));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return400_When_RequestIsNull()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: ApiRequestUrl, content: null);

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Should_Return409_When_ResourceWithProvidedUserIdAlreadyExists()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid preferenceId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new CreatePreferenceRequest(PreferenceId: preferenceId,
                UserId: UserDataInitializer.UserIds[index: 0]));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.Conflict);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: ApiRequestUrl, content: null);

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}