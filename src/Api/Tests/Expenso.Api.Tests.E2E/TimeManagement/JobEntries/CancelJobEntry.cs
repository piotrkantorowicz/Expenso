using System.Net;

using Expenso.Api.Tests.E2E.TestData.TimeManagement;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

[TestFixture]
internal sealed class CancelJobEntry : JobEntriesTestBase
{
    [Test]
    public async Task Should_Return204_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response =
            await _httpClient.DeleteAsync(
                requestUri: $"{ApiRequestUrl}/{TimeManagementDataInitializer.JobEntriesIds[index: 1]}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return404_When_JobEntryNotFound()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response =
            await _httpClient.DeleteAsync(requestUri: $"{ApiRequestUrl}/{Guid.CreateVersion7()}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response =
            await _httpClient.DeleteAsync(
                requestUri: $"{ApiRequestUrl}/{TimeManagementDataInitializer.JobEntriesIds[index: 1]}");

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}