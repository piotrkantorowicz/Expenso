using System.Net;

using Expenso.Api.Tests.E2E.TestData.BudgetSharing;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

[TestFixture]
internal sealed class ConfirmAssigningParticipant : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: Claims);

        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri: $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionRequestIds[index: 1]}/confirm",
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
            requestUri: $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionRequestIds[index: 1]}/confirm",
            content: null);

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}