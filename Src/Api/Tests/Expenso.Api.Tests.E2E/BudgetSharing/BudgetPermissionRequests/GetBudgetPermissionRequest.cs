using Expenso.Api.Tests.E2E.TestData.BudgetSharing;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

internal sealed class GetBudgetPermissionRequest : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid budgetPermissionRequestId = BudgetPermissionRequestsDataProvider.BudgetPermissionRequestIds[2];
        _httpClient.SetFakeBearerToken(_claims);
        string requestPath = $"budget-sharing/budget-permission-requests/{budgetPermissionRequestId}";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestPath);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.OK);

        // TODO: Fix this test
        // GetBudgetPermissionRequestResponse? testResultContent =
        //     await testResult.Content.ReadFromJsonAsync<GetBudgetPermissionRequestResponse>();

        // testResultContent?.Id.Should().Be(budgetPermissionRequestId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid budgetPermissionRequestId = BudgetPermissionRequestsDataProvider.BudgetPermissionRequestIds[2];
        string requestPath = $"budget-sharing/budget-permission-requests/{budgetPermissionRequestId}";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestPath);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}