using Expenso.Api.Tests.E2E.TestData.BudgetSharing;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

internal sealed class ConfirmAssigningParticipant : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        string requestPath =
            $"budget-sharing/budget-permission-requests/{BudgetPermissionDataInitializer.BudgetPermissionRequestIds[index: 1]}/confirm";

        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri: requestPath, content: null);

        // Assert
        AssertResponseNoContent(response: response);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        string requestPath =
            $"budget-sharing/budget-permission-requests/{BudgetPermissionDataInitializer.BudgetPermissionRequestIds[index: 1]}/confirm";

        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri: requestPath, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}