using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

internal sealed class GetBudgetPermissionRequest : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid budgetPermissionRequestId = BudgetPermissionDataInitializer.BudgetPermissionRequestIds[index: 2];
        _httpClient.SetFakeBearerToken(token: _claims);
        string requestPath = $"budget-sharing/budget-permission-requests/{budgetPermissionRequestId}";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseOk(response: response);

        GetBudgetPermissionRequestResponse? responseContent =
            await response.Content.ReadFromJsonAsync<GetBudgetPermissionRequestResponse>();

        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).Should().BeTrue();
        responseContent?.Id.Should().Be(expected: budgetPermissionRequestId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid budgetPermissionRequestId = BudgetPermissionDataInitializer.BudgetPermissionRequestIds[index: 2];
        string requestPath = $"budget-sharing/budget-permission-requests/{budgetPermissionRequestId}";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}