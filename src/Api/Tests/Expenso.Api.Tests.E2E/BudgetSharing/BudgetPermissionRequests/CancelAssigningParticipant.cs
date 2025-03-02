using System.Net;

using Expenso.Api.Tests.E2E.TestData.BudgetSharing;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

[TestFixture]
internal sealed class CancelAssigningParticipant : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_Return204_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissionRequestId = BudgetSharingDataInitializer.BudgetPermissionRequestIds[index: 0];

        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri: $"{ApiRequestUrl}/{budgetPermissionRequestId}/cancel", content: null);

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return404_When_ResourceNotFound()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissionRequestId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri: $"{ApiRequestUrl}/{budgetPermissionRequestId}/cancel", content: null);

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_Return422_When_ResourceNotFound()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissionRequestId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri: $"{ApiRequestUrl}/{budgetPermissionRequestId}/cancel", content: null);

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NotFound);
    }

    
    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid budgetPermissionRequestId = BudgetSharingDataInitializer.BudgetPermissionRequestIds[index: 0];

        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri: $"{ApiRequestUrl}/{budgetPermissionRequestId}/cancel", content: null);

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}