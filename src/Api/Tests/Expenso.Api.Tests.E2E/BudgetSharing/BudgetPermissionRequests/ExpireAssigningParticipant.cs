using System.Net;

using Expenso.Api.Tests.E2E.TestData.BudgetSharing;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

[TestFixture]
internal sealed class ExpireAssigningParticipant : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_Return204_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri: $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionRequestIds[index: 2]}/expire",
            content: null);

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri: $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionRequestIds[index: 2]}/expire",
            content: null);

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}