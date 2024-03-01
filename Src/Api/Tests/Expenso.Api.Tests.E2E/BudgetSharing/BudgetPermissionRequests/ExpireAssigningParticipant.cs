using Expenso.Api.Tests.E2E.TestData.BudgetSharing;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

internal sealed class ExpireAssigningParticipant : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(_claims);

        string requestPath =
            $"budget-sharing/budget-permission-requests/{BudgetPermissionRequestsDataProvider.BudgetPermissionRequestIds[2]}/expire";

        // Act
        HttpResponseMessage testResult = await _httpClient.PatchAsync(requestPath, null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        string requestPath =
            $"budget-sharing/budget-permission-requests/{BudgetPermissionRequestsDataProvider.BudgetPermissionRequestIds[2]}/expire";

        // Act
        HttpResponseMessage testResult = await _httpClient.PatchAsync(requestPath, null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}