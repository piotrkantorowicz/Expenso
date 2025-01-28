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
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri:
            $"{ApiRequestUrl}/{BudgetPermissionDataInitializer.BudgetPermissionRequestIds[index: 1]}/confirm",
            content: null);

        // Assert
        AssertResponseNoContent(response: response);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri:
            $"{ApiRequestUrl}/{BudgetPermissionDataInitializer.BudgetPermissionRequestIds[index: 1]}/confirm",
            content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}